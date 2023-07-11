using System;

namespace MusicHall.Core.Domain.Messages
{
    /// <summary>
    /// Represents the period of message delay
    /// </summary>
    public enum MessageDelayPeriod
    {
        /// <summary>
        /// Hours
        /// </summary>
        Minutes = 0,
        /// <summary>
        /// Hours
        /// </summary>
        Hours = 1,
        /// <summary>
        /// Days
        /// </summary>
        Days = 2
    }

    /// <summary>
    /// MessageDelayPeriod Extensions
    /// </summary>
    public static class MessageDelayPeriodExtensions
    {
        /// <summary>
        /// Returns message delay in minutes
        /// </summary>
        /// <param name="period">Message delay period</param>
        /// <param name="value">Value of delay send</param>
        /// <returns>Value of message delay in hours</returns>
        public static int ToMinutes(this MessageDelayPeriod period, int value)
        {
            switch (period)
            {
                case MessageDelayPeriod.Minutes:
                    return value;
                case MessageDelayPeriod.Hours:
                    return value * 60;
                case MessageDelayPeriod.Days:
                    return value * 60 * 24;
                default:
                    throw new ArgumentOutOfRangeException("MessageDelayPeriod");
            }
        }
    }
}
