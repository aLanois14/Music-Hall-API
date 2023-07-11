using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHall.Core.Domain.Messages
{
    [Table("queued_email")]
    public class QueuedEmail : BaseEntity
    {
        /// <summary>
        /// Gets or sets the priority
        /// </summary>
        [Column("priority_id")]
        public int PriorityId { get; set; }

        /// <summary>
        /// Gets or sets the From property (email address)
        /// </summary>
        [Column("from")]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the FromName property
        /// </summary>
        [Column("from_name")]
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the To property (email address)
        /// </summary>
        [Column("to")]
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the ToName property
        /// </summary>
        [Column("to_name")]
        public string ToName { get; set; }

        /// <summary>
        /// Gets or sets the ReplyTo property (email address)
        /// </summary>
        [Column("reply_to")]
        public string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the ReplyToName property
        /// </summary>
        [Column("reply_to_name")]
        public string ReplyToName { get; set; }

        /// <summary>
        /// Gets or sets the CC
        /// </summary>
        [Column("cc")]
        public string CC { get; set; }

        /// <summary>
        /// Gets or sets the BCC
        /// </summary>
        [Column("bcc")]
        public string Bcc { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        [Column("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        [Column("body")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the attachment file path (full file path)
        /// </summary>
        [Column("attachment_file_path")]
        public string AttachmentFilePath { get; set; }

        /// <summary>
        /// Gets or sets the attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.
        /// </summary>
        [Column("attachment_file_name")]
        public string AttachmentFileName { get; set; }

        /// <summary>
        /// Gets or sets the download identifier of attached file
        /// </summary>
        [Column("attachment_download_id")]
        public int AttachedDownloadId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of item creation in UTC
        /// </summary>
        [Column("created_on_utc")]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time in UTC before which this email should not be sent
        /// </summary>
        [Column("dont_send_before_date_utc")]
        public DateTime? DontSendBeforeDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the send tries
        /// </summary>
        [Column("sent_tries")]
        public int SentTries { get; set; }

        /// <summary>
        /// Gets or sets the sent date and time
        /// </summary>
        [Column("sent_on_utc")]
        public DateTime? SentOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the used email account identifier
        /// </summary>
        [Column("email_account_id")]
        public int EmailAccountId { get; set; }

        /// <summary>
        /// Gets the email account
        /// </summary>
        public virtual EmailAccount EmailAccount { get; set; }

        /// <summary>
        /// Gets or sets the priority
        /// </summary>
        public QueuedEmailPriority Priority
        {
            get
            {
                return (QueuedEmailPriority)this.PriorityId;
            }
            set
            {
                this.PriorityId = (int)value;
            }
        }
    }
}
