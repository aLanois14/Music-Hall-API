using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using MusicHall.Core.Domain.Users;
using MusicHall.Services.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MusicHall.Services.Authentication
{
    /// <summary>
    /// Represents service using cookie middleware for the authentication
    /// </summary>
    public partial class CookieAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User _cachedUser;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// /// <param name="administratorService">Administrator service</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        public CookieAuthenticationService(
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._userService = userService;
            this._httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        public virtual async void SignIn(User user, bool isPersistent)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //create claims for customer's username and email
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName));

            if (!string.IsNullOrEmpty(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, CustomCookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var cookieExpires = isPersistent ? 24 * 365 : 2;
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.Now,
                ExpiresUtc = cookieExpiresDate,
                AllowRefresh = true
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(CustomCookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            //cache authenticated customer
            _cachedUser = user;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        public virtual async void SignOut()
        {
            //reset cached member
            //_cachedMember = null;
            _cachedUser = null;

            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(CustomCookieAuthenticationDefaults.AuthenticationScheme);

        }

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <returns>Customer</returns>
        public virtual User GetAuthenticatedUser()
        {
            //whether there is a cached customer
            if (_cachedUser != null)
                return _cachedUser;

            //try to get authenticated user identity
            var authenticateResult = _httpContextAccessor.HttpContext.AuthenticateAsync(CustomCookieAuthenticationDefaults.AuthenticationScheme).Result;
            if (!authenticateResult.Succeeded)
                return null;

            User user = null;

            //try to get customer by email
            var emailClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                && claim.Issuer.Equals(CustomCookieAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
            if (emailClaim != null)
                user = _userService.GetUserByEmail(emailClaim.Value);

            //whether the found customer is available
            //if (customer == null || !customer.Active || customer.RequireReLogin || customer.Deleted )
            if (user == null)
                return null;

            //cache authenticated customer
            _cachedUser = user;

            return _cachedUser;
        }

        #endregion
    }
}
