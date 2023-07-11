using MusicHall.Core.Domain.Common;
using MusicHall.Core.Domain.Users;
using System.Collections.Generic;
using System.Linq;

namespace MusicHall.Services.Users
{
    public interface IUserService
    {
        #region Users
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        IQueryable<User> GetAllUsers();

        /// <summary>
        /// Gets an user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        User GetUserById(int userId);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        User GetUserByEmail(string email);

        User GetUserByEmailLight(string email);

        /// <summary>
        /// Get user by password token
        /// </summary>
        /// <param name="passwordToken">Password Token</param>
        /// <returns>User</returns>
        User GetUserByPasswordToken(string passwordToken);
        
        /// <summary>
        /// Create the user
        /// </summary>
        /// <param name="user">User</param>
        void InsertUser(User user);
        void InsertUsers(IList<User> users);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        void UpdateUser(User user);

        /// <summary>
        /// Delete list of users
        /// </summary>
        /// <param name="users">List of users</param>
        void DeleteUsers(IList<User> users);
        #endregion

        #region User passwords

        /// <summary>
        /// Gets User passwords
        /// </summary>
        /// <param name="userId">user identifier; pass null to load all records</param>
        /// <param name="passwordFormat">Password format; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        IList<UserPassword> GetUserPasswords(int? userId = null, PasswordFormat? passwordFormat = null, int? passwordsToReturn = null);

        /// <summary>
        /// Get current User password
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User password</returns>
        UserPassword GetCurrentPassword(int userId);

        /// <summary>
        /// Insert a user password
        /// </summary>
        /// <param name="UserPassword">User password</param>
        void InsertUserPassword(UserPassword userPassword);

        /// <summary>
        /// Insert a User password
        /// </summary>
        /// <param name="UserPassword">User password</param>
        void UpdateUserPassword(UserPassword userPassword);

        #endregion

        #region User Token
        UserToken GetUserTokenByUse(int userId, TokenUse tokenUse);

        void InsertUserToken(UserToken userToken);

        void UpdateUserToken(UserToken userToken);
        #endregion
    }
}
