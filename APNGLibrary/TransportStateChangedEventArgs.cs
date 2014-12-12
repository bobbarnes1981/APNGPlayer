using System;

namespace APNGLibrary
{
    public class TransportStateChangedEventArgs : EventArgs
    {
        public TransportState TransportState { get; private set; }

        public TransportStateChangedEventArgs(TransportState transportState)
        {
            TransportState = transportState;
        }
    }
}
