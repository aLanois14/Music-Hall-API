using MusicHall.Core;
using MusicHall.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MusicHall.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace MusicHall.Services.Users
{
    public class UserService : IUserService
    {
        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserPassword> _userPasswordRepository;
        private readonly IRepository<UserToken> _userTokenRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>        
        /// <param name="userRepository">User repository</param>
        /// <param name="userPasswordRepository">User password repository</param>
        public UserService(
            IRepository<User> userRepository, 
            IRepository<UserPassword> userPasswordRepository,
            IRepository<UserToken> userTokenRepository)
        {
            this._userRepository = userRepository;
            this._userPasswordRepository = userPasswordRepository;
            this._userTokenRepository = userTokenRepository;
        }

        #endregion

        #region User
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> GetAllUsers()
        {
            var query = _userRepository.Table;

            return query;
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        public virtual User GetUserById(int userId)
        {
            if (userId == 0)
                return null;

            var query = _userRepository.GetById(userId);

            return query;
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        public virtual User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = _userRepository.Table.Where(x => x.Email == email).FirstOrDefault();

            return query;
        }

        public virtual User GetUserByEmailLight(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query =
                _userRepository.Table.Where(x => x.Email == email).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// Get user by password token
        /// </summary>
        /// <param name="passwordToken">Password Token</param>
        /// <returns>User</returns>
        public User GetUserByPasswordToken(string passwordToken)
        {
            if (string.IsNullOrWhiteSpace(passwordToken))
                return null;

            var query = _userRepository.Table.Where(x => x.PasswordRecoveryToken == passwordToken).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.CreatedAtUtc = DateTime.Now;
            user.UpdatedAtOnUtc = DateTime.Now;

            _userRepository.Insert(user);

        }

        /// <summary>
        /// Insert a list of users
        /// </summary>
        /// <param name="users">List of users</param>
        public void InsertUsers(IList<User> users)
        {
            _userRepository.Insert(users);
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.UpdatedAtOnUtc = DateTime.Now;

            _userRepository.Update(user);

        }

        /// <summary>
        /// Delete list of users
        /// </summary>
        /// <param name="users">List of users</param>
        public void DeleteUsers(IList<User> users)
        {
            _userRepository.Delete(users);
        }
        #endregion

        #region User passwords

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordFormat">Password format; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        public virtual IList<UserPassword> GetUserPasswords(int? userId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            var query = _userPasswordRepository.Table;

            //filter by user
            if (userId.HasValue)
                query = query.Where(password => password.UserId == userId.Value);

            //filter by password format
            if (passwordFormat.HasValue)
                query = query.Where(password => password.PasswordFormatId == (int)(passwordFormat.Value));

            //get the latest passwords
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreatedAtUtc).Take(passwordsToReturn.Value);

            return query.ToList();
        }

        /// <summary>
        /// Get current user password
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User password</returns>
        public virtual UserPassword GetCurrentPassword(int userId)
        {
            if (userId == 0)
                return null;

            //return the latest password
            return GetUserPasswords(userId, passwordsToReturn: 1).FirstOrDefault();
        }

        /// <summary>
        /// Insert a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        public virtual void InsertUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            _userPasswordRepository.Insert(userPassword);

        }

        /// <summary>
        /// Update a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        public virtual void UpdateUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            _userPasswordRepository.Update(userPassword);

        }

        #endregion

        #region User Token
        public UserToken GetUserTokenByUse(int userId, TokenUse tokenUse)
        {
            if (userId == 0 || tokenUse == 0)
                return null;

            return _userTokenRepository.Table.Where(x => x.UserId == userId && x.TokenUse == tokenUse && !x.Deleted).FirstOrDefault();
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void InsertUserToken(UserToken userToken)
        {
            if (userToken == null)
                throw new ArgumentNullException(nameof(userToken));

            userToken.UpdatedAtOnUtc = DateTime.Now;
            userToken.Deleted = false;

            _userTokenRepository.Insert(userToken);

        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void UpdateUserToken(UserToken userToken)
        {
            if (userToken == null)
                throw new ArgumentNullException(nameof(userToken));

            userToken.UpdatedAtOnUtc = DateTime.Now;

            _userTokenRepository.Update(userToken);

        }
        #endregion
    }
}
