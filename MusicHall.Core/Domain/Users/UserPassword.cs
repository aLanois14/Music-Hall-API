using MusicHall.Core.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHall.Core.Domain.Users
{
    /// <summary>
    /// Represents an user password
    /// </summary>
    [Table("user_passwords")]
    public partial class UserPassword : BaseEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public UserPassword()
        {
            this.PasswordFormat = PasswordFormat.Encrypted;
        }

        /// <summary>
        /// Gets or sets the member identifier
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        [Column("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password format identifier
        /// </summary>
        [Column("password_format_id")]
        public int PasswordFormatId { get; set; }

        /// <summary>
        /// Gets or sets the password salt
        /// </summary>
        [Column("password_salt")]
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        [Column("password_format")]
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public virtual User User { get; set; }
    }
}
