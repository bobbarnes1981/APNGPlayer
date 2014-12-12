using System;
using System.Collections.Generic;
using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// PNG Image
    /// </summary>
    public class PNG
    {
        private PNGStream m_stream;

        public IHDR Header { get; private set; }

        private List<Chunk> m_chunks = new List<Chunk>();

		public PNG(string path)
			: this(new FileStream(path, FileMode.Open))
		{
		}

	    public PNG(Stream stream)
            : this(new PNGStream(stream))
	    {
	    }

	    public PNG(PNGStream stream)
	    {
	        m_stream = stream;

            // read png signature
            if (!readSignature())
            {
                Console.WriteLine("Invalid file signature.");
            }
		}

		public PNG(IHDR ihdr, IDAT idat, params Chunk[] chunks)
        {
            m_chunks.Add(ihdr);
			m_chunks.AddRange(chunks);
            m_chunks.Add(idat);
            m_chunks.Add(new IEND());
        }

        public void AppendIDAT(IDAT idat)
        {
            m_chunks[m_chunks.Count - 1] = idat;
            m_chunks.Add(new IEND());
        }

        public void ReadAllChunks()
        {
            Chunk chunk;
            do
            {
                chunk = GetNextChunk();
                m_chunks.Add(chunk);
            } while (chunk.ChunkType != ChunkType.IEND);
        }

        public Chunk GetNextChunk()
        {
            Chunk chunk = m_stream.ReadChunk();
            if (chunk.ChunkType == ChunkType.IHDR)
            {
                Header = (IHDR) chunk;
            }
            else if (m_chunks.Count == 0)
            {
                throw new Exception(string.Format("First chunk is {0} and should be IHDR", chunk.ChunkType));
            }
            m_chunks.Add(chunk);
            return chunk;
        }

        public void Save(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.CreateNew);
            Save(fileStream);
            fileStream.Close();
        }

        public void Save(Stream stream)
        {
            writeSignature(stream);
            foreach (Chunk chunk in m_chunks)
            {
                chunk.Write(stream);
            } 
        }

        private void writeSignature(Stream stream)
        {
            foreach (byte b in PNGSignature)
            {
                stream.WriteByte(b);
            }
        }

        protected bool readSignature()
        {
            foreach (byte b in PNGSignature)
            {
                if (m_stream.ReadByte() != b)
                {
                    return false;
                }
            }
            return true;
        }

        //89 50 4E 47 0D 0A 1A 0A
        public static byte[] PNGSignature
        {
            get
            {
                return new byte[]
	            {
                    0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
	            };
            }
        }
    }
}
