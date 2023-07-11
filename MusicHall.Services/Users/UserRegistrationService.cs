using MusicHall.Core;
using MusicHall.Core.Domain.Common;
using MusicHall.Core.Domain.Users;
using MusicHall.Services.Registration;
using MusicHall.Services.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHall.Services.Users
{

    public class UserRegistrationService : IUserRegistrationService
    {

        #region Fields

        private const int SALT_KEY_SIZE = 5;

        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="userService">User service</param>
        public UserRegistrationService(IUserService userService, IEncryptionService encryptionService)
        {
            this._userService = userService;
            this._encryptionService = encryptionService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether the entered password matches with a saved one
        /// </summary>
        /// <param name="userPassword">User password</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(UserPassword userPassword, string enteredPassword)
        {
            if (userPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (userPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, userPassword.PasswordSalt, "SHA512");
                    break;
            }

            if (userPassword.Password == null)
                return false;

            return userPassword.Password.Equals(savedPassword);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual UserLoginResults ValidateUser(string email, string password)
        {
            var user = _userService.GetUserByEmail( email);

            if (user == null)
                return UserLoginResults.UserNotExist;
            if (user.Disabled)
                return UserLoginResults.Deleted;

            if (!PasswordsMatch(_userService.GetCurrentPassword(user.Id), password))
            {
                return UserLoginResults.WrongPassword;
            }

            return UserLoginResults.Successful;
        }

        /// <summary>
        /// Validate user by ID
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual UserLoginResults ValidateUserById(int id, string password)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
                return UserLoginResults.UserNotExist;
            if (user.Disabled)
                return UserLoginResults.Deleted;
            if (!user.Confirmed)
                return UserLoginResults.NotActive;

            if (!PasswordsMatch(_userService.GetCurrentPassword(user.Id), password))
            {
                return UserLoginResults.WrongPassword;
            }
      
            return UserLoginResults.Successful;
        }

        /// <summary>
        /// Register customer
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual UserRegistrationResult RegisterUser(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.User == null)
                throw new ArgumentException("Can't load current member");

            var result = new UserRegistrationResult();
            if (string.IsNullOrEmpty(request.Email))
            {
                result.AddError("EmailIsNotProvided");
                return result;
            }
            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError("WrongEmail");
                return result;
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("PasswordIsNotProvided");
                return result;
            }

            //validate unique user
            if (_userService.GetUserByEmail(request.Email) != null)
            {
                result.AddError("Account.Register.Errors.EmailAlreadyExists");
                return result;
            }

            //at this point request is valid
            request.User.Email = request.Email;

            var userPassword = new UserPassword
            {
                User = request.User,
                PasswordFormat = request.PasswordFormat,
                CreatedAtUtc = DateTime.Now
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    {
                        var saltKey = _encryptionService.CreateSaltKey(SALT_KEY_SIZE);
                        userPassword.PasswordSalt = saltKey;
                        userPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, "SHA15");
                    }
                    break;
            }

            _userService.InsertUserPassword(userPassword);

            if (request.User.Id <= 0)
            {
                _userService.InsertUser(request.User);
            }

            return result;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new ChangePasswordResult();
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError("EmailIsNotProvided");
                return result;
            }
            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("PasswordIsNotProvided");
                return result;
            }

            var user = _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                result.AddError("Account.ChangePassword.Errors.EmailNotFound");
                return result;
            }

            if (request.ValidateRequest)
            {
                //request isn't valid
                if (!PasswordsMatch(_userService.GetCurrentPassword(user.Id), request.OldPassword))
                {
                    result.AddError("OldPasswordDoesntMatch");
                    return result;
                }
            }

            //at this point request is valid
            var customerPassword = new UserPassword
            {
                User = user,
                PasswordFormat = request.NewPasswordFormat,
                CreatedAtUtc = DateTime.Now
            };
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                    customerPassword.Password = request.NewPassword;
                    break;
                case PasswordFormat.Encrypted:
                    customerPassword.Password = _encryptionService.EncryptText(request.NewPassword);
                    break;
                case PasswordFormat.Hashed:
                    {
                        var saltKey = _encryptionService.CreateSaltKey(SALT_KEY_SIZE);
                        customerPassword.PasswordSalt = saltKey;
                        customerPassword.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, "SHA512");
                    }
                    break;
            }
            _userService.InsertUserPassword(customerPassword);

            user.PasswordRecoveryTokenDateGenerated = null;
            _userService.UpdateUser(user);

            return result;
        }

        #endregion
    }
}
