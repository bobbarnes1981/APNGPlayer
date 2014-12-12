using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace APNGLibrary
{
    /// <summary>
    /// Animated PNG streamer
    /// </summary>
    public class APNGStreamer : IStreamer, IDisposable
    {
        // TODO: cache previous frames to enable rewind

        /// <summary>
        /// Raised when a frame a new frame is available
        /// </summary>
        public event EventHandler<FrameUpdateEventArgs> FrameUpdateEvent;

        /// <summary>
        /// Raised when the elapsed time is updated
        /// </summary>
        public event EventHandler<ElapsedTimeUpdateEventArgs> ElapsedTimeUpdateEvent;

        /// <summary>
        /// Raised when the transport state is changed
        /// </summary>
        public event EventHandler<TransportStateChangedEventArgs> TransportStateChangedEvent;

        /// <summary>
        /// Raised when the speed is changed
        /// </summary>
        public event EventHandler<SpeedChangedEventArgs> SpeedChangedEvent;

        /// <summary>
        /// Gets a clone of the current frame image
        /// </summary>
        public Image Image
        {
            get
            {
                if (imageCurr == null)
                {
                    return null;
                }
                return (Image)imageCurr.Clone();
            }
        }

        /// <summary>
        /// Frame count target for 'PlayTo'
        /// </summary>
        private int m_frameCounterTarget = 0;

        private int m_frameCount;

        /// <summary>
        /// Frame count
        /// </summary>
        public int FrameCount
        {
            get { return m_frameCount; }
            private set
            {
                m_frameCount = value;
                raiseFrameUpdateEvent(m_frameCount);
            }
        }

        /// <summary>
        /// Elapsed time target for 'PlayTo'
        /// </summary>
        private TimeSpan m_elapsedTimeTarget = new TimeSpan();

        private TimeSpan m_elapsedTime;

        /// <summary>
        /// Elapsed time
        /// </summary>
        public TimeSpan ElapsedTime 
        {
            get { return m_elapsedTime; }
            private set
            {
                m_elapsedTime = value;
                raiseElapsedTimeUpdateEvent(m_elapsedTime);
            }
        }

        /// <summary>
        /// Animated png file
        /// </summary>
        private APNG m_apng;

        /// <summary>
        /// Speed multiplier field
        /// </summary>
        private float m_speed;

        /// <summary>
        /// Field multiplier
        /// </summary>
        public float Speed
        {
            get { return m_speed; }
            set 
            { 
                m_speed = value;
                raiseSpeedChangedEvent(m_speed);
            }
        }

        /// <summary>
        /// Transport state field
        /// </summary>
        private TransportState m_transportState;

        /// <summary>
        /// Transport state
        /// </summary>
        public TransportState TransportState
        {
            get { return m_transportState; }
            private set
            {
                m_transportState = value;
                raiseTransportStateChangedEvent(m_transportState);
            }
        }

        /// <summary>
        /// Constructs a new APNGStreamer
        /// </summary>
        /// <param name="path"></param>
        public APNGStreamer(string path)
        {
            m_apng = new APNG(path);
            m_speed = 1;
            m_frameCount = 0;
            ElapsedTime = new TimeSpan();
        }

        /// <summary>
        /// Play the stream
        /// </summary>
        public void Play()
        {
            TransportState = TransportState.Play;
            new Thread(fetchFrames).Start();
        }

        /// <summary>
        /// Play the stream to the specified frame count
        /// </summary>
        /// <param name="frameCount"></param>
        public void PlayToFrameCount(int frameCount)
        {
            Pause();
            m_frameCounterTarget = frameCount;
            Play();
        }

        /// <summary>
        /// Play the stream to the specified elapsed time
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void PlayToElapsedTime(TimeSpan elapsedTime)
        {
            Pause();
            m_elapsedTimeTarget = elapsedTime;
            Play();
        }

        /// <summary>
        /// Pause the stream
        /// </summary>
        public void Pause()
        {
            TransportState = TransportState.Pause;
        }

        /// <summary>
        /// Previous image store for restoring
        /// </summary>
        private Image imagePrev;

        /// <summary>
        /// Current image store for cloning
        /// </summary>
        private Image imageCurr;

        /// <summary>
        /// Previous frame store for getting disposal operation
        /// </summary>
        private Frame framePrev;

        /// <summary>
        /// Fetch frames from file
        /// </summary>
        private void fetchFrames()
        {
            Frame frame;
            // do while playing and not finished
            while (!reachedTarget() && TransportState == TransportState.Play)
            {
                // get next frame from file
                frame = m_apng.GetNextFrame();
                // if at end of file
                if (frame == null)
                {
                    break;
                }
                // update the elapsed time
                ElapsedTime = ElapsedTime.Add(new TimeSpan(frame.FrameControl.DelayTicks));

                // overlay the new frame on the current one
                overlayFrame(frame);
                // if not skipping, wait for correct time
                if (Speed > 0.1f) // TODO: change this comparison if required
                {
                    Thread.Sleep((int)(frame.FrameControl.DelayMilliseconds * Speed));
                }
            }
            // pause and clear targets
            TransportState = TransportState.Pause;
            m_elapsedTimeTarget = TimeSpan.Zero;
            m_frameCounterTarget = 0;
        }

        private bool reachedTarget()
        {
            // if we are using a target and we have reached the target
            return (m_elapsedTimeTarget != TimeSpan.Zero && ElapsedTime >= m_elapsedTimeTarget) ||
                   (m_frameCounterTarget != 0 && FrameCount >= m_frameCounterTarget);
        }

        /// <summary>
        /// Overlay the new frame on the current frame
        /// </summary>
        /// <param name="frameCurr"></param>
        private void overlayFrame(Frame frameCurr)
        {
            // init image if empty
            if (imageCurr == null)
            {
                // assume first image is a full image and set image size
                imageCurr = new Bitmap((int)frameCurr.FrameControl.Width, (int)frameCurr.FrameControl.Height);
            }

            // get graphics for current image
            using (Graphics graphics = Graphics.FromImage(imageCurr))
            {
                // dispose the current image
                if (framePrev != null)
                {
                    switch (framePrev.FrameControl.DisposeOperation)
                    {
                        case DisposeOperation.Previous:
                            // load the last image
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(imagePrev, 0, 0);
                            break;
                        case DisposeOperation.Background:
                            // remove the previous frame area
                            throw new NotImplementedException();
                            break;
                        case DisposeOperation.None:
                            // no disposal necessary
                            break;
                    }
                }

                // load the next frame
                MemoryStream stream = new MemoryStream();
                frameCurr.FrameImage.Save(stream);
                Image overlayImage = Image.FromStream(stream);
                // make black transparent - a bit hacky
                if (framePrev != null)
                {
                    ((Bitmap)overlayImage).MakeTransparent(Color.Black);
                }

                // overlay the new image
                switch (frameCurr.FrameControl.BlendOperation)
                {
                    case BlendOperation.Over:
                        graphics.CompositingMode = CompositingMode.SourceOver;
                        break;
                    case BlendOperation.Source:
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        break;
                }
                graphics.DrawImage(overlayImage, frameCurr.FrameControl.XOffset, frameCurr.FrameControl.YOffset);
            }

            // set the previous image and frame
            imagePrev = (Image)imageCurr.Clone();
            framePrev = frameCurr;

            // update the frame count
            FrameCount = FrameCount + 1;
        }

        private void raiseFrameUpdateEvent(int frameCount)
        {
            if (FrameUpdateEvent != null)
            {
                FrameUpdateEvent(this, new FrameUpdateEventArgs(frameCount));
            }
        }

        private void raiseElapsedTimeUpdateEvent(TimeSpan elapsedTime)
        {
            if (ElapsedTimeUpdateEvent != null)
            {
                ElapsedTimeUpdateEvent(this, new ElapsedTimeUpdateEventArgs(elapsedTime));
            }
        }

        private void raiseSpeedChangedEvent(float speed)
        {
            if (SpeedChangedEvent != null)
            {
                SpeedChangedEvent(this, new SpeedChangedEventArgs(speed));
            }
        }

        private void raiseTransportStateChangedEvent(TransportState transportState)
        {
            if (TransportStateChangedEvent != null)
            {
                TransportStateChangedEvent(this, new TransportStateChangedEventArgs(transportState));
            }
        }

        public void Dispose()
        {
            if (imageCurr != null)
            {
                imageCurr.Dispose();
            }
            if (imagePrev != null)
            {
                imagePrev.Dispose();
            }
        }
    }
}
