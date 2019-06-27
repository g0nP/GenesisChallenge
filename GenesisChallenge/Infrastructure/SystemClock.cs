using System;

namespace GenesisChallenge.Infrastructure
{
    public interface ISystemClock
    {
        DateTime GetCurrentTime();
    }

    public class SystemClock : ISystemClock
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
