using System;
using System.Collections.Generic;
using System.Linq;
using MusicHall.Core;
using MusicHall.Core.Domain.Messages;

namespace MusicHall.Services.Message
{
    /// <summary>
    /// Message template service
    /// </summary>
    public partial class MessageTemplateService : IMessageTemplateService
    {
        #region Fields

        private readonly IRepository<MessageTemplate> _messageTemplateRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public MessageTemplateService(IRepository<MessageTemplate> messageTemplateRepository)
        {
            this._messageTemplateRepository = messageTemplateRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        public virtual void InsertMessageTemplate(MessageTemplate messageTemplate)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            messageTemplate.CreatedAtUtc = DateTime.Now;
            messageTemplate.UpdatedAtOnUtc = DateTime.Now;

            _messageTemplateRepository.Insert(messageTemplate);
        }

        /// <summary>
        /// Updates a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        public virtual void UpdateMessageTemplate(MessageTemplate messageTemplate)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            messageTemplate.UpdatedAtOnUtc = DateTime.Now;

            _messageTemplateRepository.Update(messageTemplate);
        }

        /// <summary>
        /// Gets a message template
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <returns>Message template</returns>
        public virtual MessageTemplate GetMessageTemplateById(int messageTemplateId)
        {
            if (messageTemplateId == 0)
                return null;

            return _messageTemplateRepository.GetById(messageTemplateId);
        }

        /// <summary>
        /// Gets message templates by the system name
        /// </summary>
        /// <param name="messageTemplateSystemName">Message template system name</param>
        /// <returns>List of message templates</returns>
        public virtual IList<MessageTemplate> GetMessageTemplatesBySystemName(string messageTemplateSystemName)
        {
            if (string.IsNullOrWhiteSpace(messageTemplateSystemName))
                throw new ArgumentException(nameof(messageTemplateSystemName));

            var templates = _messageTemplateRepository.Table
                .Where(messageTemplate => messageTemplate.Name.Equals(messageTemplateSystemName))
                .OrderBy(messageTemplate => messageTemplate.Id).ToList();

            return templates;
        }

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <returns>Message template list</returns>
        public virtual IList<MessageTemplate> GetAllMessageTemplates()
        {
            var templates = _messageTemplateRepository.Table
                .OrderBy(messageTemplate => messageTemplate.Id).ToList();

            return templates;
        }

        #endregion
    }
}
