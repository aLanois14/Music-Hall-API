namespace MusicHall.Core.Domain.Messages
{
    /// <summary>
    /// Represents priority of queued email
    /// </summary>
    public enum QueuedEmailPriority
    {
        /// <summary>
        /// Low
        /// </summary>
        Low = 0,

        /// <summary>
        /// Medium
        /// </summary>
        Medium = 5,

        /// <summary>
        /// High
        /// </summary>
        High = 10
    }
}