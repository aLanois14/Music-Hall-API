using MusicHall.Core.Domain.Users;
using System;
using System.Collections.Generic;

namespace MusicHall.Services.Message
{
    public interface IMessageTokenService
    {
        #region Methods

        /// <summary>
        /// Adds tokens for multiple purposes
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="userSending"></param>
        /// <param name="userReceiving"></param>
        void AddBaseToken(IList<Token> tokens, User userSending, User userReceiving);

        /// <summary>
        /// Adds tokens for multiple purposes
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="receivingEmail"></param>
        /// <param name="receivingName"></param>
        void AddBaseToken(IList<Token> tokens, string receivingEmail, string receivingName);
    
        #endregion Methods
    }
}
