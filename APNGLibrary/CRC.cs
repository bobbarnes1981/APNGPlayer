using System.IO;

//http://www.libpng.org/pub/png/spec/1.2/PNG-CRCAppendix.html

namespace APNGLibrary
{
    /// <summary>
    /// CRC Sum
    /// </summary>
    public class CRC
    {
        /// <summary>
        /// Table of CRCs of all 8-bit messages.
        /// </summary>
        private ulong[] m_crcTable = new ulong[256];
   
        /// <summary>
        /// Flag: has the table been computed? Initially false.
        /// </summary>
        private bool m_crcTableComputed = false;
        
        /// <summary>
        /// Make the table for a fast CRC.
        /// </summary>
        private void makeCrcTable()
        {
            ulong c;
            int n, k;
   
            for (n = 0; n < 256; n++)
            {
                c = (ulong) n;
                for (k = 0; k < 8; k++)
                {
                    if ((c & 1) > 0)
                    {
                        c = 0xedb88320L ^ (c >> 1);
                    }
                    else
                    {
                        c = c >> 1;
                    }
                }
                m_crcTable[n] = c;
            }
            m_crcTableComputed = true;
        }
   
        /// <summary>
        /// Update a running CRC with the bytes buf[0..len-1]--the CRC
        /// should be initialized to all 1's, and the transmitted value
        /// is the 1's complement of the final running CRC (see the
        /// crc() routine below)).
        /// </summary>
        /// <param name="crc"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        private ulong updateCrc(ulong crc, byte[] buf)
        {
            ulong c = crc;
            int n;

            if (!m_crcTableComputed)
            {
                makeCrcTable();
            }
            for (n = 0; n < buf.Length; n++)
            {
                c = m_crcTable[(c ^ buf[n]) & 0xff] ^ (c >> 8);
            }
            return c;
        }
   
        /// <summary>
        /// Return CRC of byte array
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public ulong Calculate(byte[] buf)
        {
            return updateCrc(0xffffffffL, buf) ^ 0xffffffffL;
        }

        /// <summary>
        /// Return CRC of chunk
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public ulong Calculate(Chunk chunk)
        {
            MemoryStream memoryStream = new MemoryStream();
            chunk.Write(memoryStream, true);
            byte[] buffer = memoryStream.ToArray();
            return Calculate(buffer);
        }
    }
}
