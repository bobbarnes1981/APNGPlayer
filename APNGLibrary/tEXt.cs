using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Text chunk
    /// </summary>
	public class tEXt : Chunk
	{
		public string Keyword { get; private set; }
		public string Text { get; private set; }

		public tEXt (int length)
			: base(length, ChunkType.tEXt)
		{
		}

		protected override void load (Stream stream)
		{
            Keyword = new BinStream(stream).ReadString();
            Text = new BinStream(stream).ReadString(Length - (Keyword.Length + 1));
		}

	    protected override void write(Stream stream)
	    {
            new BinStream(stream).WriteString(Keyword);
	        stream.WriteByte(0x00);
            new BinStream(stream).WriteString(Text);
	    }

	    public override string ToString ()
		{
			return string.Format ("{0}[tEXt: keyword={1}, text={2}]", base.ToString(), Keyword, Text);
		}
	}
}

