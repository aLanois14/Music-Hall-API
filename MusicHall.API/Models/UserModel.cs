using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHall.API.Models
{
    public class UserModel
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int Activity { get; set; }
        public JwtModel Jwt { get; set; }
    }

    public class JwtModel
    {
        public string auth_token { get; set; }
        public string refresh_token { get; set; }
        public int? expires_in { get; set; }
        public string id { get; set; }
    }
}
