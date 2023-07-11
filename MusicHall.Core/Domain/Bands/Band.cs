using MusicHall.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicHall.Core.Domain.Bands
{
    [Table("bands")]
    public class Band : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("creator_id")]
        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }

        #region Navigation Properties
        public virtual User Creator { get; set; }
        #endregion
    }
}
