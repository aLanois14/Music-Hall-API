using Microsoft.AspNetCore.Http;

namespace MusicHall.Services.Authentication
{
    /// <summary>
    /// Default values related to cookie-based authentication handler
    /// </summary>
    public static class CustomCookieAuthenticationDefaults
    {
        /// <summary>
        /// The default value used for authentication scheme
        /// </summary>
        public const string AuthenticationScheme = "Cookies";
        public const string AuthenticationAdminScheme = "Cookies";

        /// <summary>
        /// The prefix used to provide a default cookie name
        /// </summary>
        public static readonly string CookiePrefix = ".Web.";
        public static readonly string CookieAdminPrefix = ".Admin.";

        /// <summary>
        /// The issuer that should be used for any claims that are created
        /// </summary>
        public static readonly string ClaimsIssuer = "custom";

        /// <summary>
        /// The default value for the login path
        /// </summary>
        public static readonly PathString LoginPath = new PathString("/users/session/login");
        public static readonly PathString LoginAdminPath = new PathString("/session/login");

        /// <summary>
        /// The default value used for the logout path
        /// </summary>
        public static readonly PathString LogoutPath = new PathString("/users/session/logout");
        public static readonly PathString LogoutAdminPath = new PathString("/session/logout");

        /// <summary>
        /// The default value for the access denied path
        /// </summary>
        public static readonly PathString AccessDeniedPath = new PathString("/page-not-found");

        /// <summary>
        /// The default value of the return URL parameter
        /// </summary>
        public static readonly string ReturnUrlParameter = "";
    }
}
