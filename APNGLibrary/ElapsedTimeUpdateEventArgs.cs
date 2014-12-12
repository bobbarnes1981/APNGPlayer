using System;

namespace APNGLibrary
{
    public class ElapsedTimeUpdateEventArgs : EventArgs
    {
        public TimeSpan ElapsedTime { get; private set; }

        public ElapsedTimeUpdateEventArgs(TimeSpan elapsedTime)
        {
            ElapsedTime = elapsedTime;
        }
    }
}
