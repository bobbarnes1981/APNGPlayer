using System;
using System.IO;
using APNGLibrary;

namespace APNGTool
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length != 1) 
			{
				Console.WriteLine ("Usage: APNGTool <filename>");
				return;
			}

            // load image
			APNG apng = new APNG (args [0]);

            // generate directory path
		    string path = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(args[0]));

            // create directory if not exist
		    if (!Directory.Exists(path))
		    {
		        Directory.CreateDirectory(path);
		    }

            // save all frames
		    apng.SaveFrames(path);
		}
	}
}
