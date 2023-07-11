using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicHall.API.Models;
using MusicHall.Core.Domain.Publications;
using MusicHall.Core.Domain.Users;
using MusicHall.Services.Publications;
using MusicHall.Services.Users;
using MusicHall.Web.Controllers;

namespace MusicHall.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private readonly AppSettingModel _appSettingModel;
        private readonly IPublicationService _publicationService;

        private readonly IUserService _userService;

        public PublicationController(
            AppSettingModel appSettingModel,
            IPublicationService publicationService,
            IUserService userService)
        {
            _appSettingModel = appSettingModel;
            _publicationService = publicationService;
            _userService = userService;
        }

        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                List<PublicationModel> publications =
                    _publicationService.GetAll().OrderByDescending(x => x.CreatedAtUtc)
                    .Select(x => new PublicationModel
                    {
                        Id= x.Id,
                        Guid = x.Guid.ToString(),
                        Title = x.Title,
                        Description = x.Description,
                        BandId = x.BandId,
                        PublicationFiles = x.PublicationFiles.Select(pp => new PublicationFileModel
                        {
                            FileString = pp.File,
                            Guid = pp.Guid.ToString(),
                            Name = pp.Name,
                            Type = pp.Type
                        }).ToList()
                    }).ToList();

               foreach(var publication in publications)
               {
                    if(publication.PublicationFiles != null)
                    {
                        foreach (var file in publication.PublicationFiles)
                        {
                            if (!String.IsNullOrEmpty(file.FileString))
                            {
                                if(file.Type == "audio")
                                {
                                    string path = Path.Combine(_appSettingModel.Uploads.Publication_Audio, file.Guid.ToString() + "/");
                                    string fullPath = Path.Combine(path + file.FileString);
                                    byte[] bytes = System.IO.File.ReadAllBytes(fullPath);

                                    file.FileString = "data:audio/mpeg;base64," + Convert.ToBase64String(bytes);
                                }
                                else if(file.Type == "image")
                                {
                                    string path = Path.Combine(_appSettingModel.Uploads.Publication_Pictures, file.Guid.ToString() + "/");
                                    using (Image image = Image.FromFile(Path.Combine(path + file.FileString)))
                                    {
                                        using (MemoryStream m = new MemoryStream())
                                        {
                                            image.Save(m, image.RawFormat);
                                            byte[] imageBytes = m.ToArray();

                                            file.FileString = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                                        }
                                    }
                                }
                                
                            }
                        }
                    }
                    
               }

                return Ok(publications);
            }
            catch(Exception ex)
            {
                return StatusCode(400);
            }
        }

        [Route("NewPublication")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult NewPublication([FromForm] PublicationModel publicationModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Files = Request.Form.Files;
                    User currentUser = _userService.GetUserByEmailLight(User.FindFirst("email").Value);
                    Publication publication = new Publication();
                    publication.Title = publicationModel.Title;
                    publication.Description = publicationModel.Description;
                    //publication.BandId = publicationModel.BandId;
                    publication.UserId = currentUser.Id;
                    publication.Guid = Guid.NewGuid();
                    publication.PublicationFiles = new List<PublicationFile>();

                    if(Files != null)
                    {
                        foreach (var file in Files)
                        {
                            PublicationFile publicationFile = new PublicationFile();
                            publicationFile.Guid = Guid.NewGuid();
                            publicationFile.Name = file.FileName;
                            if (file.ContentType.Contains("image"))
                            {
                                publicationFile.Type = "image";
                                string path = Path.Combine(_appSettingModel.Uploads.Publication_Pictures, publicationFile.Guid.ToString() + "/");
                                publicationFile.File = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                                try
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                                    using (FileStream fileStream = new FileStream(Path.Combine(path, publicationFile.File), FileMode.Create))
                                    {
                                        file.CopyTo(fileStream);
                                    }
                                }
                                catch
                                {
                                    return StatusCode(400);
                                }
                            }
                            else if (file.ContentType.Contains("audio"))
                            {
                                publicationFile.Type = "audio";
                                string path = Path.Combine(_appSettingModel.Uploads.Publication_Audio, publicationFile.Guid.ToString() + "/");
                                publicationFile.File = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                                try
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                                    using (FileStream fileStream = new FileStream(Path.Combine(path, publicationFile.File), FileMode.Create))
                                    {
                                        file.CopyTo(fileStream);
                                    }
                                }
                                catch
                                {
                                    return StatusCode(400);
                                }
                            }
                            
                            publication.PublicationFiles.Add(publicationFile);
                        }
                    }
                 
                    _publicationService.Create(publication);

                    publicationModel.Id = publication.Id;
                    publicationModel.Guid = publication.Guid.ToString();
                    publicationModel.PublicationFiles = new List<PublicationFileModel>();

                    foreach(PublicationFile publicationFile in publication.PublicationFiles)
                    {
                        PublicationFileModel publicationFileModel = new PublicationFileModel();
                        publicationFileModel.Type = publicationFile.Type;
                        publicationFileModel.Name = publicationFile.Name;
                        publicationFileModel.Guid = publicationFile.Guid.ToString();

                        if (!String.IsNullOrEmpty(publicationFile.File))
                        {
                            if (publicationFile.Type == "audio")
                            {
                                string path = Path.Combine(_appSettingModel.Uploads.Publication_Audio, publicationFile.Guid.ToString() + "/");
                                string fullPath = Path.Combine(path + publicationFile.File);
                                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);

                                publicationFileModel.FileString = "data:audio/mpeg;base64," + Convert.ToBase64String(bytes);
                            }
                            else if (publicationFile.Type == "image")
                            {
                                string path = Path.Combine(_appSettingModel.Uploads.Publication_Pictures, publicationFile.Guid.ToString() + "/");
                                using (Image image = Image.FromFile(Path.Combine(path + publicationFile.File)))
                                {
                                    using (MemoryStream m = new MemoryStream())
                                    {
                                        image.Save(m, image.RawFormat);
                                        byte[] imageBytes = m.ToArray();

                                        publicationFileModel.FileString = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                                    }
                                }
                            }

                        }

                        publicationModel.PublicationFiles.Add(publicationFileModel);
                    }

                    return Ok(publicationModel);
                }
                catch
                {
                    return StatusCode(400);
                }
            }
            return Ok();
        }
    }
}
