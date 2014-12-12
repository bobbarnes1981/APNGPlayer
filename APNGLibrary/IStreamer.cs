using System;
using System.Drawing;

namespace APNGLibrary
{
    /// <summary>
    /// IStreamer interface
    /// </summary>
    public interface IStreamer
    {
        event EventHandler<FrameUpdateEventArgs> FrameUpdateEvent;
        event EventHandler<ElapsedTimeUpdateEventArgs> ElapsedTimeUpdateEvent;
        event EventHandler<TransportStateChangedEventArgs> TransportStateChangedEvent;
        event EventHandler<SpeedChangedEventArgs> SpeedChangedEvent;

        Image Image { get; }
        
        float Speed { get; set; }
        TransportState TransportState { get; }
        int FrameCount { get; }
        TimeSpan ElapsedTime { get; }
        
        void Play();
        void PlayToFrameCount(int frameCount);
        void PlayToElapsedTime(TimeSpan elapsedTime);
        void Pause();
    }
}
