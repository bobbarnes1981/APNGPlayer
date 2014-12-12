using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Transparency chunk
    /// </summary>
	public class tRNS : Chunk
	{
		public byte[] Data { get; private set; }

		public tRNS(int length)
			: base(length, ChunkType.tRNS)
		{
		}

		protected override void load (Stream stream)
		{
            Data = new BinStream(stream).ReadBytes(Length);
		}

		protected override void write(Stream stream)
		{
            new BinStream(stream).WriteBytes(Data);
		}

		public override string ToString ()
		{
			return string.Format("{0}[tRNS: Data={1}]", base.ToString(), Data);
		}
	}
}

