using Microsoft.Extensions.Configuration;
using MusicHall.Core;
using MusicHall.Core.Domain.Messages;
using MusicHall.Core.Domain.Users;
using MusicHall.Services.Security;
using MusicHall.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicHall.Services.Message
{
    public class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly IEmailAccountService _emailAccountService;
        private readonly IEncryptionService _encryptionService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenService _messageTokenService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ITokenizer _tokenizer;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public WorkflowMessageService(
            IEmailAccountService emailAccountService,
            IEncryptionService encryptionService,
            IMessageTemplateService messageTemplateService,
            IMessageTokenService messageTokenService,
            IQueuedEmailService queuedEmailService,
            ITokenizer tokenizer,
            IUserService userService,
            IWorkContext workContext,
            IConfiguration configuration)
        {
            this._emailAccountService = emailAccountService;
            this._encryptionService = encryptionService;
            this._messageTemplateService = messageTemplateService;
            this._messageTokenService = messageTokenService;
            this._queuedEmailService = queuedEmailService;
            this._tokenizer = tokenizer;
            this._userService = userService;
            this._workContext = workContext;
            this._configuration = configuration;
        }

        #endregion

        #region Send Mail (Generic function)

        /// <summary>
        /// Sends mail according to email template
        /// </summary>
        /// <param name="userSending">Sender</param>
        /// <param name="userReceiving">Receiver</param>
        /// <param name="emailTemplateSystemName">emailTemplateSystemName</param>
        /// <returns></returns>
        public IList<int> SendMessage(User userSending, User userReceiving, string emailTemplateSystemName)
        {
            #region Check

            if (userSending == null)
                throw new ArgumentNullException(nameof(userSending));
            if (userReceiving == null)
                throw new ArgumentNullException(nameof(userReceiving));
            if (string.IsNullOrEmpty(emailTemplateSystemName))
                throw new ArgumentNullException(nameof(emailTemplateSystemName));

            #endregion

            #region Tokens

            var commonTokens = new List<Token>();
            _messageTokenService.AddBaseToken(commonTokens, userSending, userReceiving);

            #endregion

            IList<MessageTemplate> messageTemplates = new List<MessageTemplate>();

            switch (emailTemplateSystemName)
            {
                case MessageTemplateSystemNames.UserPasswordRecovery:
                    messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserPasswordRecovery);

                    userReceiving.PasswordRecoveryToken = Guid.NewGuid().ToString();
                    userReceiving.PasswordRecoveryTokenDateGenerated = DateTime.UtcNow;
                    _userService.UpdateUser(userReceiving);

                    //_messageTokenService.AddPasswordRecoveryToken(commonTokens, userReceiving.PasswordRecoveryToken);

                    break;
                case MessageTemplateSystemNames.UserWelcome:
                    messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserWelcome);

                    break;
            }

            if (!messageTemplates.Any())
                return new List<int>();

            return messageTemplates.Select(messageTemplate =>
            {
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate);

                var tokens = new List<Token>(commonTokens);

                var toEmail = userReceiving.Email;
                var toName = userReceiving.FirstName + " " + userReceiving.LastName;

                return SendNotification(messageTemplate, emailAccount, tokens, toEmail, toName);
            }).ToList();
        }

        #endregion Send Mail (Generic function)

        #region Workflow

        
        #endregion

        #region Send Notification

        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <param name="replyToEmailAddress">"Reply to" email</param>
        /// <param name="replyToName">"Reply to" name</param>
        /// <param name="fromEmail">Sender email. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="fromName">Sender name. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="subject">Subject. If specified, then it overrides subject of a message template</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendNotification(MessageTemplate messageTemplate,
            EmailAccount emailAccount, IEnumerable<Token> tokens,
            string toEmailAddress, string toName,
            string attachmentFilePath = null, string attachmentFileName = null,
            string replyToEmailAddress = null, string replyToName = null,
            string fromEmail = null, string fromName = null, string subject = null)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            if (emailAccount == null)
                throw new ArgumentNullException(nameof(emailAccount));

            //retrieve message template data
            var bcc = messageTemplate.BccEmailAddresses;
            if (string.IsNullOrEmpty(subject))
                subject = messageTemplate.Subject;
            var body = messageTemplate.Body;

            //Replace subject and body tokens 
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            //limit name length

            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                From = emailAccount.Email,
                FromName = !string.IsNullOrEmpty(fromName) ? fromName : emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                AttachedDownloadId = messageTemplate.AttachedDownloadId,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
                DontSendBeforeDateUtc = !messageTemplate.DelayBeforeSend.HasValue ? null
                    : (DateTime?)(DateTime.UtcNow + TimeSpan.FromMinutes(messageTemplate.DelayPeriod.ToMinutes(messageTemplate.DelayBeforeSend.Value)))
            };

            _queuedEmailService.InsertQueuedEmail(email);
            return email.Id;
        }

        #endregion Send Notification

        #region Utilities

        /// <summary>
        /// Get active message templates by the system name
        /// </summary>
        /// <param name="messageTemplateSystemName">Message template system name</param>
        /// <returns>List of message templates</returns>
        protected virtual IList<MessageTemplate> GetActiveMessageTemplates(string messageTemplateSystemName)
        {
            //get message templates by the name
            var messageTemplates = _messageTemplateService.GetMessageTemplatesBySystemName(messageTemplateSystemName);

            //no template found
            if (!messageTemplates?.Any() ?? true)
                return new List<MessageTemplate>();

            //filter active templates
            messageTemplates = messageTemplates.Where(messageTemplate => messageTemplate.IsActive).ToList();

            return messageTemplates;
        }

        /// <summary>
        /// Get EmailAccount to use with a message templates by the system name
        /// </summary>
        /// <param name="messageTemplateSystemName">Message template system name</param>
        /// <returns>EmailAccount</returns>
        protected virtual EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate)
        {
            var emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = _emailAccountService.GetEmailAccountById(emailAccountId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            return emailAccount;
        }

        #endregion    
    }
}
