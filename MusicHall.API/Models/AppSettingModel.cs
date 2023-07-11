using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHall.API.Models
{
    public class AppSettingModel
    {
        public JwtIssuerSetting JwtIssuerOptions { get; set; }
        public UploadsSetting Uploads { get; set; }
        public string APP_LINK { get; set; }
    }

    public class JwtIssuerSetting
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

    public class UploadsSetting
    {
        public string Publication_Pictures { get; set; }
        public string Publication_Audio { get; set; }
        public string Profile_Pictures { get; set; }
    }
}
