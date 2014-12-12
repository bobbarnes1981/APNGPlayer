
namespace APNGLibrary
{
    /// <summary>
    /// Blend operation enumeration
    /// </summary>
    public enum BlendOperation : byte
    {
        /// <summary>
        /// all color components of the frame, including alpha, overwrite the current contents of the frame's output buffer region.
        /// </summary>
        Source,
 
        /// <summary>
        /// the frame should be composited onto the output buffer based on its alpha, using a simple OVER operation as described in the "Alpha Channel Processing" section of the PNG specification [PNG-1.2]. Note that the second variation of the sample code is applicable.
        /// </summary>
        Over
    }
}
