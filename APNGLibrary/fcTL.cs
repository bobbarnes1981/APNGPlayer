using System;
using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Frame control chunk
    /// </summary>
	public class fcTL : Chunk
	{
		public uint SequenceNumber { get; private set; }
		public uint Width { get; private set; }
		public uint Height { get; private set; }
		public uint XOffset { get; private set; }
		public uint YOffset { get; private set; }
		public ushort DelayNum { get; private set; }
		public ushort DelayDen { get; private set; }
		public DisposeOperation DisposeOperation { get; private set; }
		public BlendOperation BlendOperation { get; private set; }

        public long DelayTicks
        {
            get
            {
                if (DelayNum == 0)
                {
                    // DelayDen 100ths of a second (in ticks)
                    return (long)(0.01 * DelayDen * 10000000);
                }
                // calculate fraction of a second in ticks
                return (long)(((double)DelayNum / (double)DelayDen) * 10000000);
            }
        }

        public long DelayMilliseconds
        {
            get { return DelayTicks / 10000; }
        }
        
	    public fcTL (int length)
			: base(length, ChunkType.fcTL)
		{
		}

		protected override void load (Stream stream)
		{
            SequenceNumber = new BinStream(stream).ReadUInt();
            Width = new BinStream(stream).ReadUInt();
            Height = new BinStream(stream).ReadUInt();
            XOffset = new BinStream(stream).ReadUInt();
            YOffset = new BinStream(stream).ReadUInt();
            DelayNum = new BinStream(stream).ReadShort();
            DelayDen = new BinStream(stream).ReadShort();
			DisposeOperation = (DisposeOperation)stream.ReadByte ();
			BlendOperation = (BlendOperation)stream.ReadByte ();
		}

	    protected override void write(Stream stream)
        {
            new BinStream(stream).WriteUInt(SequenceNumber);
            new BinStream(stream).WriteUInt(Width);
            new BinStream(stream).WriteUInt(Height);
            new BinStream(stream).WriteUInt(XOffset);
            new BinStream(stream).WriteUInt(YOffset);
            new BinStream(stream).WriteShort(DelayNum);
            new BinStream(stream).WriteShort(DelayDen);
            stream.WriteByte((byte)DisposeOperation);
            stream.WriteByte((byte)BlendOperation);
	    }

	    public override string ToString ()
		{
			return string.Format ("{0}[fcTL: sequenceNumber={1}, width={2}, height={3}, xOffset={4}, yOffset={5}, delayNum={6}, delayDen={7}, disposeOp={8}, blendOp={9}]", base.ToString(), SequenceNumber, Width, Height, XOffset, YOffset, DelayNum, DelayDen, DisposeOperation, BlendOperation);
		}
	}
}

