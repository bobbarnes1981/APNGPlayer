using System.IO;

namespace APNGLibrary
{
    /// <summary>
    /// Binary stream
    /// </summary>
	public class BinStream
	{
		public Stream BaseStream { get; private set; }

		public BinStream (Stream stream)
		{
			BaseStream = stream;
		}

		public string ReadString(int length)
		{
			string result = string.Empty;
			for (int i = length; i > 0; i--)
			{
				result += (char)BaseStream.ReadByte ();
			}
			return result;
		}

		public void WriteString(string input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				BaseStream.WriteByte((byte)input[i]);
			}
		}

		public string ReadString()
		{
			string result = string.Empty;
			bool done = false;
			do
			{
				int character = BaseStream.ReadByte();
				if (character == 0)
				{
					done = true;
				}
				else
				{
					result += (char)character;
				}
			} while (!done);
			return result;
		}

		public int ReadInt()
		{
			int result = 0;
			for (int i = 4; i > 0; i--)
			{
				result += (BaseStream.ReadByte () << ((i - 1) * 8));
			}
			return result;
		}

		public void WriteInt(int input)
		{
			for (int i = 4; i > 0; i--)
			{
				BaseStream.WriteByte((byte)(input >> ((i - 1) * 8)));
			}
		}

		public uint ReadUInt()
		{
			uint result = 0;
			for (int i = 4; i > 0; i--)
			{
				result += (uint)(BaseStream.ReadByte() << ((i - 1) * 8));
			}
			return result;
		}

		public void WriteUInt(uint input)
		{
			for (int i = 4; i > 0; i--)
			{
				BaseStream.WriteByte((byte)(input >> ((i - 1) * 8)));
			}
		}

		public short ReadShort()
		{
			short result = 0;
			for (int i = 2; i > 0; i--)
			{
				result += (short)(BaseStream.ReadByte() << ((i - 1) * 8));
			}
			return result;
		}

		public void WriteShort(short input)
		{
			for (int i = 2; i > 0; i--)
			{
				BaseStream.WriteByte((byte)(input >> ((i - 1) * 8)));
			}
		}

	    public byte ReadByte()
	    {
	        return (byte)BaseStream.ReadByte();
	    }

		public byte[] ReadBytes(int length)
		{
			byte[] result = new byte[length];
			for (int i = 0; i < length; i++)
			{
				result [i] = (byte)BaseStream.ReadByte();
			}
			return result;
		}

	    public void WriteBytes(byte[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                BaseStream.WriteByte(input[i]);
            }
	    }
	}
}

