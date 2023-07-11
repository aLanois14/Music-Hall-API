using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHall.API.Models
{
    public class PublicationModel
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile File { get; set; }

        public int? BandId { get; set; }
        public int? UserId { get; set; }
        public List<PublicationFileModel> PublicationFiles { get; set; }
    }

    public class PublicationFileModel
    {
        public IFormFile File { get; set; }
        public string FileString { get; set; }
        public string Guid { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
