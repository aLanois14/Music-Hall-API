using MusicHall.Core.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHall.Core.Domain.Users
{
    /// <summary>
    /// Represents an customer
    /// </summary>
    [Table("users")]
    public class User : BaseEntity
    {

        /// <summary>
        /// Gets or sets the customer GUID
        /// </summary>
        [Column("guid")]
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the civility Id
        /// </summary>
        [Column("civility_id")]
        [ForeignKey("Civility")]
        public int CivilityId { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        [Column("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        [Column("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [Column("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        [Column("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the mobile
        /// </summary>
        [Column("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the confirm option
        /// </summary>
        [Column("confirmed")]
        public bool Confirmed { get; set; }

        /// <summary>
        /// Gets or sets the disable option
        /// </summary>
        [Column("disabled")]
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets a password recovery token
        /// </summary>
        [Column("password_recovery_token")]
        public string PasswordRecoveryToken { get; set; }

        /// <summary>
        /// Gets or sets the date password token generated
        /// </summary>
        [Column("password_recovery_token_date_generated")]
        public DateTime? PasswordRecoveryTokenDateGenerated { get; set; }

        [Column("avatar")]
        public string Avatar { get; set; }

        #region Navigation Properties

        public virtual Civility Civility { get; set; }

        #endregion
    }
}
