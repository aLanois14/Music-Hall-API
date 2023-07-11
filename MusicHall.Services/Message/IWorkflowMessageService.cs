using MusicHall.Core.Domain.Users;
using System;
using System.Collections.Generic;

namespace MusicHall.Services.Message
{
    public interface IWorkflowMessageService
    {
        /// <summary>
        /// Sends mail according to email template
        /// </summary>
        /// <param name="userSending">Sender</param>
        /// <param name="userReceiving">Receiver</param>
        /// <param name="emailTemplateSystemName">emailTemplateSystemName</param>
        /// <returns></returns>
        IList<int> SendMessage(User userSending, User userReceiving, string emailTemplateSystemName); 

    }
}
