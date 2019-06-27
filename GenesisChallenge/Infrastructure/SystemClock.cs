using System;

namespace GenesisChallenge.Infrastructure
{
    /// <summary>
    /// Exposes the system's clock
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        /// Gets the current time from the system's clock
        /// </summary>
        DateTime GetCurrentTime();
    }

    /// <summary>
    /// Implements ISystemClock
    /// </summary>
    public class SystemClock : ISystemClock
    {
        /// <summary>
        /// Gets the current time from the system's clock in UTC
        /// </summary>
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
