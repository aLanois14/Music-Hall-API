using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicHall.Core.Domain.Publications
{
    [Table("publication_files")]
    public class PublicationFile : BaseEntity
    {
        [Column("guid")]
        public Guid Guid { get; set; }

        [Column("file")]
        public string File { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("publication_id")]
        [ForeignKey(nameof(Publication))]
        public int PublicationId { get; set; }

        #region Navigation Properties
        public virtual Publication Publication { get; set; }
        #endregion
    }
}
