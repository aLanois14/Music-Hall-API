using MusicHall.Core.Domain.Bands;
using MusicHall.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicHall.Core.Domain.Publications
{
    [Table("publications")]
    public class Publication : BaseEntity
    {
        private ICollection<PublicationFile> _publicationFiles;


        [Column("guid")]
        public Guid Guid { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }

        [Column("band_id")]
        [ForeignKey(nameof(Band))]
        public int? BandId { get; set; }

        #region Navigation Properties
        public virtual Band Band { get; set; }

        public virtual User User { get; set; }

        public ICollection<PublicationFile> PublicationFiles
        {
            get { return _publicationFiles ?? (_publicationFiles = new List<PublicationFile>()); }
            set { _publicationFiles = value; }
        }
        #endregion
    }
}
