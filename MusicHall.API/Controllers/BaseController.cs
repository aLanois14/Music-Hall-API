using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicHall.Core;

namespace MusicHall.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected readonly ILogger<BaseController> Logger;

        /// <summary>
        /// CUrrent specific environment
        /// </summary>
        protected readonly ProjectEnvironment Environment;

        public BaseController(ILogger<BaseController> logger, IOptionsMonitor<ProjectEnvironment> env)
        {
            Logger = logger;
            Environment = env.CurrentValue;
        }
    }
}
