using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Animated PNG
    /// </summary>
	public class APNG : PNG
	{
		public APNG(string path)
			: base(path)
		{
		}

	    public APNG(Stream stream)
            : base(stream)
	    {
	    }

	    public APNG(PNGStream stream)
            : base(stream)
		{
		}

        tRNS transparency;
        fcTL lastFrameCtrl;

	    public Frame GetNextFrame()
	    {
	        Frame frame = null;
	        Chunk currentChunk;
	        do
            {
                currentChunk = GetNextChunk();
	            switch (currentChunk.ChunkType)
	            {
	                case ChunkType.fcTL:
	                    lastFrameCtrl = (fcTL) currentChunk;
	                    if (frame != null)
	                    {
	                        return frame;
	                    }
	                    break;
	                case ChunkType.fdAT:
	                    if (frame == null)
                        {
                            frame = new Frame(lastFrameCtrl,
                                new PNG(new IHDR(Header, lastFrameCtrl.Width, lastFrameCtrl.Height),
                                    new IDAT(((fdAT)currentChunk).Data), transparency));   
	                    }
	                    else
	                    {
	                        frame.AppendIDAT(new IDAT(((fdAT)currentChunk).Data));
	                    }
	                    break;
	                case ChunkType.IDAT:
	                    if (frame == null)
                        {
                            frame = new Frame(lastFrameCtrl,
                                   new PNG(new IHDR(Header, lastFrameCtrl.Width, lastFrameCtrl.Height),
                                       (IDAT)currentChunk, transparency));
	                    }
	                    else
	                    {
                            frame.AppendIDAT((IDAT)currentChunk);
	                    }
	                    break;
	                case ChunkType.tRNS:
	                    transparency = (tRNS) currentChunk;
	                    break;
	            }
	        } while (currentChunk.ChunkType != ChunkType.IEND);

	        return null;
	    }
        
	    public void SaveFrames(string path)
	    {
	        long elapsedTicks = 0;
	        Frame frame;
	        do
	        {
	            frame = GetNextFrame();
	            if (frame != null)
                {
                    frame.FrameImage.Save(Path.Combine(path, string.Format("{0}.png", elapsedTicks)));
                    elapsedTicks += frame.FrameControl.DelayTicks;
                }
	        } while (frame != null);
        }
	}
}

