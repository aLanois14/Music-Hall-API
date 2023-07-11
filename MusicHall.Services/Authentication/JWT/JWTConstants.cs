using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHall.Services.Authentication.JWT
{
    public static class JWTConstants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id", Email = "email";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }
    }
}
