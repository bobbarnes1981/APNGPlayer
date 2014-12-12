
namespace APNGLibrary
{
    /// <summary>
    /// Animation frame
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// Frame control chunk
        /// </summary>
        public fcTL FrameControl { get; private set; }

        /// <summary>
        /// Frame PNG image
        /// </summary>
        public PNG FrameImage { get; private set; }

        public Frame(fcTL frameControl, PNG image)
        {
            FrameControl = frameControl;
            FrameImage = image;
        }

        public void AppendIDAT(IDAT idat)
        {
            FrameImage.AppendIDAT(idat);
        }
    }
}
