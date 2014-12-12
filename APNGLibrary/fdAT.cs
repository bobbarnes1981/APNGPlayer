using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Frame data chunk
    /// </summary>
	public class fdAT : Chunk
	{
		public uint SequenceNumber { get; private set; }

		public byte[] Data { get; private set; }

		public fdAT (int length)
			: base(length, ChunkType.fdAT)
		{
		}

		protected override void load (Stream stream)
		{
			SequenceNumber = new BinStream(stream).ReadUInt();
            Data = new BinStream(stream).ReadBytes(Length - 4); // TODO: data format?
		}

	    protected override void write(Stream stream)
        {
            new BinStream(stream).WriteUInt(SequenceNumber);
	        new BinStream(stream).WriteBytes(Data);
	    }

	    public override string ToString ()
		{
			return string.Format ("{0}[fdAT: SequenceNumber={1}, Data={2}]", base.ToString(), SequenceNumber, Data);
		}
	}
}

