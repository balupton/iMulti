using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using iTunesLib;
using System.IO;

namespace iMulti
{
    public partial class Program
    {
        public static void PerformRED()
        {
            Console.Clear();
            Console.WriteLine("Performing RED (Remove Empty Directories)");
            Console.Title = "Performing RED (Remove Empty Directories)";

            Console.WriteLine("\r\nSelect your iTunes Media Library folder.");
            if (!PickDirectory())
            {
                Console.WriteLine("\r\nOperation Cancelled, Press any key to continue.");
                Console.ReadKey(true);
                return;
            }
            Console.WriteLine("Chosen Directory: " + DirLocation);

            Console.WriteLine("\r\nScanning of directories will now start.\r\nPress any key to continue.\r\n");
            Console.ReadKey(true);

            DirectoryInfo dir = new DirectoryInfo(DirLocation);
            //int s = dir.GetFiles("*.mp3");


            //Console.ReadKey(true);
        }
    }
}