
namespace APNGLibrary
{
    /// <summary>
    /// Dispose operatin enumeration
    /// </summary>
    public enum DisposeOperation : byte
    {
        /// <summary>
        /// no disposal is done on this frame before rendering the next; the contents of the output buffer are left as is.
        /// </summary>
        None,

        /// <summary>
        /// the frame's region of the output buffer is to be cleared to fully transparent black before rendering the next frame.
        /// </summary>
        Background,

        /// <summary>
        /// the frame's region of the output buffer is to be reverted to the previous contents before rendering the next frame.
        /// </summary>
        Previous
    }
}
