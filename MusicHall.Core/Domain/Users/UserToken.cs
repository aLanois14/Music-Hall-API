using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicHall.Core.Domain.Users
{
    [Table("user_token")]
    public class UserToken : BaseEntity
    {
        /// <summary>
        /// Gets or sets the member identifier
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("token_use")]
        public TokenUse TokenUse { get; set; }


        [Column("deleted")]
        public bool Deleted { get; set; }

        #region Navigation properties
        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public virtual User User { get; set; }
        #endregion    
    }

    public enum TokenUse
    {
        Confirmation = 1
    }
}
