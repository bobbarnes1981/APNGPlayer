using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Animation control chunk
    /// </summary>
	public class acTL : Chunk
	{
		public int NumFrames { get; private set; }
		public int NumPlays { get; private set; }

		public acTL (int length)
			: base(length, ChunkType.acTL)
		{
		}

		protected override void load (Stream stream)
		{
            NumFrames = new BinStream(stream).ReadInt();
            NumPlays = new BinStream(stream).ReadInt();
		}

		protected override void write(Stream stream)
        {
            new BinStream(stream).WriteInt(NumFrames);
            new BinStream(stream).WriteInt(NumPlays);
	    }

	    public override string ToString ()
		{
			return string.Format ("{0}[acTL: numFrames={1}, numPlays={2}]", base.ToString(), NumFrames, NumPlays);
		}
	}
}

