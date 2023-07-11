using MusicHall.Core.Domain.Messages;
using System.Collections.Generic;

namespace MusicHall.Services.Message
{
    public partial interface IMessageTemplateService
    {
        /// <summary>
        /// Inserts a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        void InsertMessageTemplate(MessageTemplate messageTemplate);

        /// <summary>
        /// Updates a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        void UpdateMessageTemplate(MessageTemplate messageTemplate);

        /// <summary>
        /// Gets a message template by identifier
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <returns>Message template</returns>
        MessageTemplate GetMessageTemplateById(int messageTemplateId);

        /// <summary>
        /// Gets message templates by the name
        /// </summary>
        /// <param name="messageTemplateSystemName">Message template system name</param>
        /// <returns>List of message templates</returns>
        IList<MessageTemplate> GetMessageTemplatesBySystemName(string messageTemplateName);

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <returns>Message template list</returns>
        IList<MessageTemplate> GetAllMessageTemplates();
    }
}
