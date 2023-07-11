using MusicHall.Core.Domain.Users;

namespace MusicHall.Core
{
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        User CurrentUser { get; set; }
    }
}
