using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHall.Core.Domain.Messages
{
    [Table("email_account")]
    public class EmailAccount : BaseEntity
    {
        /// <summary>
        /// Gets or sets an email address
        /// </summary>
        [Column("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets an email display name
        /// </summary>
        [Column("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets an email host
        /// </summary>
        [Column("host")]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets an email port
        /// </summary>
        [Column("port")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets an email user name
        /// </summary>
        [Column("user_name")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets an email password
        /// </summary>
        [Column("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
        /// </summary>
        [Column("enable_ssl")]
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether the default system credentials of the application are sent with requests.
        /// </summary>
        [Column("use_default_credentials")]
        public bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// Gets a friendly email account name
        /// </summary>
        public string FriendlyName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.DisplayName))
                    return this.Email + " (" + this.DisplayName + ")";
                return this.Email;
            }
        }
    }
}
