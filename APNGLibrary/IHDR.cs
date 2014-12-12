//Color    Allowed    Interpretation
//Type    Bit Depths
//0       1,2,4,8,16  Each pixel is a grayscale sample.
//2       8,16        Each pixel is an R,G,B triple.
//3       1,2,4,8     Each pixel is a palette index;
//						a PLTE chunk must appear.
//4       8,16        Each pixel is a grayscale sample,
//						followed by an alpha sample.
//6       8,16        Each pixel is an R,G,B triple,
//						followed by an alpha sample.
using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Header
    /// </summary>
	public class IHDR : Chunk
	{
		public uint Width { get; private set; }
		public uint Height { get; private set; }
		public byte BitDepth { get; private set; }
		public ColourType ColourType { get; private set; }
		public CompressionMethod CompressionMethod { get; private set; }
		public FilterMethod FilterMethod { get; private set; }
		public InterlaceMethod InterlaceMethod { get; private set; }

		public IHDR (int length)
			: base(length, ChunkType.IHDR)
		{
		}

        public IHDR(IHDR ihdr, uint width, uint height)
            : this(0x0D)
        {
            Width = width;
            Height = height;
            BitDepth = ihdr.BitDepth;
            ColourType = ihdr.ColourType;
            CompressionMethod = ihdr.CompressionMethod;
            FilterMethod = ihdr.FilterMethod;
            InterlaceMethod = ihdr.InterlaceMethod;
            CRC = (uint)new CRC().Calculate(this);
        }

		protected override void load (Stream stream)
		{
            Width = new BinStream(stream).ReadUInt();
            Height = new BinStream(stream).ReadUInt();
			BitDepth = (byte)stream.ReadByte ();
			ColourType = (ColourType)stream.ReadByte ();
			CompressionMethod = (CompressionMethod)stream.ReadByte ();
			FilterMethod = (FilterMethod)stream.ReadByte ();
			InterlaceMethod = (InterlaceMethod)stream.ReadByte ();
		}

	    protected override void write(Stream stream)
        {
            new BinStream(stream).WriteUInt(Width);
            new BinStream(stream).WriteUInt(Height);
            stream.WriteByte(BitDepth);
            stream.WriteByte((byte)ColourType);
            stream.WriteByte((byte)CompressionMethod);
            stream.WriteByte((byte)FilterMethod);
            stream.WriteByte((byte)InterlaceMethod);
	    }

	    public override string ToString ()
		{
			return string.Format ("{0}[IHDR: Width={1}, Height={2}, BitDepth={3}, ColourType={4}, CompressionMethod={5}, FilterMethod={6}, interlaceMethod={7}]", base.ToString(), Width, Height, BitDepth, ColourType, CompressionMethod, FilterMethod, InterlaceMethod);
		}
	}
}

