using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Unknown chunk - for unimplemented chunks
    /// </summary>
	public class Unknown : Chunk
	{
		public byte[] Data { get; private set; }

		public Unknown(int length, ChunkType chunkType)
			: base(length, chunkType)
		{
		}

		protected override void load (Stream stream)
		{
			Data = new BinStream(stream).ReadBytes (Length);
		}

        protected override void write(Stream stream)
        {
            new BinStream(stream).WriteBytes(Data);
	    }

	    public override string ToString ()
		{
			return string.Format ("{0}[Unknown: Data={1}]", base.ToString(), Data);
		}
	}
}

