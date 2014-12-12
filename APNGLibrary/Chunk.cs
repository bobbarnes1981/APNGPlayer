using System;
using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Abstract PNG chunk
    /// </summary>
	public abstract class Chunk
	{
        /// <summary>
        /// CRC sum
        /// </summary>
	    private uint m_crc;
        
        public static Chunk Factory(int length, ChunkType chunkType, Stream stream)
		{
			Chunk chunk;
			switch (chunkType)
            {
                // APNG Chunks
                case ChunkType.acTL:
                    chunk = new acTL(length);
                    break;
                case ChunkType.fcTL:
                    chunk = new fcTL(length);
                    break;
                case ChunkType.fdAT:
                    chunk = new fdAT(length);
                    break;
                // PNG Chunks
			    case ChunkType.IDAT:
				    chunk = new IDAT(length);
				    break;
			    case ChunkType.IEND:
				    chunk = new IEND (length);
				    break;
			    case ChunkType.IHDR:
				    chunk = new IHDR (length);
				    break;
			    case ChunkType.tEXt:
				    chunk = new tEXt (length);
				    break;
			    case ChunkType.tRNS:
				    chunk = new tRNS (length);
				    break;
			    default:
				    chunk = new Unknown (length, chunkType);
				    break;
            }
            chunk.load(stream);
            chunk.CRC = new PNGStream(stream).ReadUInt();
            return chunk;
		}
		
        protected Chunk(int length, ChunkType chunkType)
		{
			Length = length;
			ChunkType = chunkType;
		}
		
        /// <summary>
        /// Chunk data length
        /// </summary>
        public int Length { get; private set; }
		
        /// <summary>
        /// Chunk type
        /// </summary>
        public ChunkType ChunkType { get; private set; }

        /// <summary>
        /// CRC Sum
        /// </summary>
	    public uint CRC
	    {
	        get { return m_crc; }
	        protected set
	        {
                uint calcCrc = (uint)new CRC().Calculate(this);
	            if (value != calcCrc)
	            {
	                throw new Exception(string.Format("Provided CRC ({0:x8}) but calculated CRC ({1:x8}).", value, calcCrc));
	            }
	            m_crc = value;
	        }
	    }

        /// <summary>
        /// Load from stream
        /// </summary>
        /// <param name="stream"></param>
		protected abstract void load(Stream stream);
	    
        /// <summary>
        /// Write to stream
        /// </summary>
        /// <param name="stream"></param>
		protected abstract void write(Stream stream);

        /// <summary>
        /// Write to stream
        /// </summary>
        /// <param name="stream"></param>
		public void Write(Stream stream)
	    {
			Write(stream, false);
	    }

        /// <summary>
        /// Write to stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="forCrc"></param>
		public void Write(Stream stream, bool forCrc)
	    {
			PNGStream pngStream = new PNGStream (stream); 
            if (!forCrc)
            {
				pngStream.WriteInt(Length);
            }
			pngStream.WriteChunkType(ChunkType);
			write(stream);
	        if (!forCrc)
	        {
				pngStream.WriteUInt(CRC);
	        }
	    }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns></returns>
		public override string ToString ()
		{
			return string.Format ("[Chunk: Length={0}, ChunkType={1}, CRC=0x{2:x8}]", Length, ChunkType, CRC);
		}
	}
}

