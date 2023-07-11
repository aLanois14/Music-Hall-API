using Microsoft.Extensions.Configuration;
using MusicHall.Core.Domain.Users;
using System.Collections.Generic;

namespace MusicHall.Services.Message
{
    public class MessageTokenService : IMessageTokenService
    {
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public MessageTokenService(
            IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Adds tokens for multiple purposes
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="userSending"></param>
        /// <param name="userReceiving"></param>
        public void AddBaseToken(IList<Token> tokens, User userSending, User userReceiving)
        {
            //User Sending
            tokens.Add(new Token("Sender.Name", (userSending.Civility != null ?  userSending.Civility.Title + " " : "") + userSending.FirstName + " " + userSending.LastName));
            tokens.Add(new Token("Sender.Name",  userSending.FirstName + " " + userSending.LastName));
            tokens.Add(new Token("Sender.FirstName", userSending.FirstName));
            tokens.Add(new Token("Sender.LastName", userSending.LastName));
            tokens.Add(new Token("Sender.Email", userSending.Email));

            //User receiving
            tokens.Add(new Token("Receiver.Name", (userReceiving.Civility != null ? userReceiving.Civility.Title + " " : "") + userReceiving.FirstName + " " + userReceiving.LastName));
            tokens.Add(new Token("Receiver.FirstName", userReceiving.FirstName));
            tokens.Add(new Token("Receiver.LastName", userReceiving.LastName));
            tokens.Add(new Token("Receiver.Email", userReceiving.Email));
        }

        /// <summary>
        /// Adds tokens for multiple purposes
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="receivingEmail"></param>
        /// <param name="receivingName"></param>
        public void AddBaseToken(IList<Token> tokens, string receivingEmail, string receivingName)
        {
            tokens.Add(new Token("Receiver.Name", receivingName));
            tokens.Add(new Token("Receiver.Email", receivingEmail));
        }

        #endregion Methods

        #region Utilities
        #endregion
    }
}
