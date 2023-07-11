using MusicHall.Services.Authentication.JWT;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicHall.API.Helpers
{
    public class JwtHelper
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "email").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                refresh_token = jwtFactory.GenerateRefreshToken(),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
