using Microsoft.AspNetCore.Http;
using MusicHall.Core;
using MusicHall.Core.Domain.Users;
using MusicHall.Services.Authentication;
using MusicHall.Services.Users;
using System;

namespace MusicHall.Services
{
    public class WorkContext : IWorkContext
    {
        #region Const

        private const string ADMINISTRATOR_COOKIE_NAME = ".MusicHall.Administrator";
        private const string USER_COOKIE_NAME = ".MusicHall.User";

        #endregion

        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        private User _cachedUser;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="administratorService">Administrator service</param>
        /// <param name="userService">User service</param>
        public WorkContext(IHttpContextAccessor httpContextAccessor,
            IAuthenticationService authenticationService,
            IUserService userService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._authenticationService = authenticationService;
            this._userService = userService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get roadbook customer cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetUserCookie()
        {
            return _httpContextAccessor.HttpContext?.Request?.Cookies[USER_COOKIE_NAME];
        }

        /// <summary>
        /// Get roadbook admin cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetAdministratorCookie()
        {
            return _httpContextAccessor.HttpContext?.Request?.Cookies[ADMINISTRATOR_COOKIE_NAME];
        }

        /// <summary>
        /// Set customer cookie
        /// </summary>
        /// <param name="customerGuid">Guid of the customer</param>
        protected virtual void SetUserCookie(Guid guid)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(USER_COOKIE_NAME);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (guid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(USER_COOKIE_NAME, guid.ToString(), options);
        }

        /// <summary>
        /// Set roadbook customer cookie
        /// </summary>
        /// <param name="customerGuid">Guid of the customer</param>
        protected virtual void SetAdministratorCookie(Guid guid)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(ADMINISTRATOR_COOKIE_NAME);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (guid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(ADMINISTRATOR_COOKIE_NAME, guid.ToString(), options);
        }

        #endregion

        #region Properties
      
        /// <summary>
        /// Gets or sets the current member
        /// </summary>
        public virtual User CurrentUser
        {
            get
            {
                //whether there is a cached value
                if (_cachedUser != null)
                    return _cachedUser;

                User user = null;

                if (user == null)
                {
                    //try to get registered user
                    user = _authenticationService.GetAuthenticatedUser();
                }

                if (user != null)
                {
                    if (!user.Disabled)
                    {
                        //set customer cookie
                        SetUserCookie(user.Guid);

                        //cache the found customer
                        _cachedUser = user;
                    }
                }
                return _cachedUser;
            }
            set
            {
                SetUserCookie(value.Guid);
                _cachedUser = value;
            }
        }

        #endregion
    }
}
