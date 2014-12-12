using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Data chunk
    /// </summary>
    public class IDAT : Chunk
    {
        public byte[] Data { get; private set; }

        public IDAT(int length)
			: base(length, ChunkType.IDAT)
		{
		}

        public IDAT(byte[] data)
            : this(data.Length)
        {
            Data = data;
            CRC = (uint)new CRC().Calculate(this);
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
            return string.Format("{0}[IDAT: Data={1}]", base.ToString(), Data);
		}
    }
}
