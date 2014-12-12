using System;

namespace APNGLibrary
{
    public class SpeedChangedEventArgs : EventArgs
    {
        public float Speed { get; private set; }

        public SpeedChangedEventArgs(float speed)
        {
            Speed = speed;
        }
    }
}
