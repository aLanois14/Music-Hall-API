using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MusicHall.API.Helpers;
using MusicHall.API.Models;
using MusicHall.Core.Domain.Common;
using MusicHall.Core.Domain.Users;
using MusicHall.Services.Authentication.JWT;
using MusicHall.Services.Registration;
using MusicHall.Services.Security;
using MusicHall.Services.Users;
using Newtonsoft.Json;

namespace MusicHall.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IJwtFactory _jwtFactory ;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IEncryptionService _encryptionService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AppSettingModel _appSettingModel;

        public AuthenticationController(
            IUserService userService, 
            IUserRegistrationService userRegistrationService, 
            IJwtFactory jwtFactory,           
            IEncryptionService encryptionService,
            IOptions<JwtIssuerOptions> jwtOptions,
            IWebHostEnvironment hostingEnvironment,
            AppSettingModel appSettingModel)
        {
            _userService = userService;
            _userRegistrationService = userRegistrationService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _encryptionService = encryptionService;
            _hostingEnvironment = hostingEnvironment;
            _appSettingModel = appSettingModel;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] BaseAuthLoginModel model)
        {
            User user = _userService.GetUserByEmailLight(model.Email);

            if (user == null)
                return StatusCode(400, "userNotExist");

            ClaimsIdentity identity = await GetClaimsIdentity(model.Email, model.Password);

            if (identity == null)
                return StatusCode(400, "wrongPassword");

            string jwt = await JwtHelper.GenerateJwt(identity, _jwtFactory, model.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            JwtModel deserializedJwt = System.Text.Json.JsonSerializer.Deserialize<JwtModel>(jwt);


            _userService.UpdateUser(user);

            UserModel currentUser = new UserModel();
            currentUser.Id = user.Id;
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.Jwt = deserializedJwt;

            _userService.UpdateUser(user);

            return Ok(currentUser);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] JwtModel jwtModel)
        {
            if (jwtModel is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = jwtModel.auth_token;
            string refreshToken = jwtModel.refresh_token;
            var claim = _jwtFactory.GetPrincipalFromExpiredToken(accessToken);
            string email = claim.FindFirst("email")?.Value;
            var user = _userService.GetUserByEmailLight(email);

            ClaimsIdentity identity = await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(email, user.Id.ToString()));

            string jwt = await JwtHelper.GenerateJwt(identity, _jwtFactory, user.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            JwtModel deserializedJwt = System.Text.Json.JsonSerializer.Deserialize<JwtModel>(jwt);

            _userService.UpdateUser(user);

            return Ok(deserializedJwt);
        }

        [Route("ValidateEmailAvailability/{email}")]
        [HttpGet]
        public IActionResult ValidateEmail(string email)
        {
            User user = _userService.GetUserByEmail(email);

            if (user != null)
            {
                return StatusCode(400, "Email déjà utilisé");
            }

            return Ok();
        }

        [Route("Register")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Register([FromForm] RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User();
                    user.FirstName = register.FirstName;
                    user.LastName = register.LastName;
                    user.Email = register.Email;
                    user.Guid = Guid.NewGuid();
                    user.CivilityId = 1;

                    if (register.File != null)
                    {
                        string path = Path.Combine(_appSettingModel.Uploads.Profile_Pictures, user.Guid.ToString() + "/");
                        user.Avatar = Guid.NewGuid().ToString() + Path.GetExtension(register.File.FileName);

                        try
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                            using (FileStream fileStream = new FileStream(Path.Combine(path, user.Avatar), FileMode.Create))
                            {
                                register.File.CopyTo(fileStream);
                            }
                        }
                        catch
                        {
                            return StatusCode(400);
                        }
                    }

                    UserRegistrationRequest userRegister = new UserRegistrationRequest(user, register.Email, string.Empty, register.Password, PasswordFormat.Encrypted);

                    _userRegistrationService.RegisterUser(userRegister);

                    ClaimsIdentity identity = await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(user.Email, user.Id.ToString()));
                    string jwt = await JwtHelper.GenerateJwt(identity, _jwtFactory, user.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
                    JwtModel deserializedJwt = System.Text.Json.JsonSerializer.Deserialize<JwtModel>(jwt);

                    UserModel currentUser = new UserModel();
                    currentUser.Id = user.Id;
                    currentUser.FirstName = user.FirstName;
                    currentUser.LastName = user.LastName;
                    currentUser.Email = user.Email;
                    currentUser.Jwt = deserializedJwt;

                    return Ok(currentUser);
                }
                catch
                {
                    return StatusCode(400);
                }
            }
            return StatusCode(400);
        }

        [Route("ConfirmEmail")]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel model)
        {
            User user = _userService.GetUserByEmailLight(model.Email);

            if (user == null)
                return StatusCode(400);

            // Token verification
            UserToken confirmationToken = _userService.GetUserTokenByUse(user.Id, TokenUse.Confirmation);
            if (confirmationToken != null && confirmationToken.Token == model.Token)
            {
                // Updating user
                user.Confirmed = true;

                // Deleting confirmation token
                confirmationToken.Deleted = true;
                _userService.UpdateUserToken(confirmationToken);

                //TODO
                ClaimsIdentity identity = await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(user.Email, user.Id.ToString()));
                string jwt = await JwtHelper.GenerateJwt(identity, _jwtFactory, user.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

                //await _mailjetService.SendWithTemplate(user, user.Lang == "fr" ? TemplateIdsFormat.WelcomeEmailFr : TemplateIdsFormat.WelcomeEmailEn, new JObject { { "linkUrl", _appSettingModel.APP_LINK } });

                JwtModel deserializedJwt = System.Text.Json.JsonSerializer.Deserialize<JwtModel>(jwt);

                UserModel currentUser = new UserModel();
                currentUser.Id = user.Id;
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Jwt = deserializedJwt;
                
                _userService.UpdateUser(user);

                return Ok(currentUser);
            }

            return StatusCode(400);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            User userToVerify = _userService.GetUserByEmail(email);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            UserLoginResults validateUser = _userRegistrationService.ValidateUser(email, password);
            if (validateUser == UserLoginResults.Successful)
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(email, userToVerify.Id.ToString()));
            }
            else
            {
                return null;
            }
        }
    }
}
