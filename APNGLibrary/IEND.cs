using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// End chunk
    /// </summary>
	public class IEND : Chunk
	{
		public IEND (int length)
			: base(length, ChunkType.IEND)
		{
		}

	    public IEND()
            : this(0)
        {
            CRC = (uint)new CRC().Calculate(this);
	    }

		protected override void load (Stream fileStream)
		{
		}

	    protected override void write(Stream fileStream)
	    {
	    }

	    public override string ToString ()
		{
			return string.Format ("{0}[IEND: ]", base.ToString());
		}
	}
}

