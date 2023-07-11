using MusicHall.Core;
using MusicHall.Core.Domain.Publications;
using MusicHall.Services.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHall.Services.Publications
{
    public class PublicationService : CRUDService<Publication>, IPublicationService
    {
        public PublicationService(IRepository<Publication> repository)
            : base(repository)
        {
        }
    }
}
