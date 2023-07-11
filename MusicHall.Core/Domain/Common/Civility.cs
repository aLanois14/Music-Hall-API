using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHall.Core.Domain.Common
{
    [Table("civilities")]
    public class Civility : BaseEntity
    {
        [Column("title")]
        public string Title { get; set; }
    }
}
