using System;

namespace APNGLibrary
{
    public class FrameUpdateEventArgs : EventArgs
    {
        public int FrameCount { get; private set; }

        public FrameUpdateEventArgs(int frameCount)
        {
            FrameCount = frameCount;
        }
    }
}
