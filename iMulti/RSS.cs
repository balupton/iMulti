using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using iTunesLib;

namespace iMulti
{
    public partial class Program
    {
        public static string theSuffix = "";
        public static void PerformRSS()
        {
            Console.Clear();
            Console.WriteLine("Performing RSS (Remove a Suffix from Songs)");
            Console.Title = "Performing RSS (Remove a Suffix from Songs)\r\n";
            Console.WriteLine("Please type the Suffix you would like to use:");
            theSuffix = Console.ReadLine().ToUpper();
            Console.WriteLine("Scanning will now start.\r\n");
            //string dt1 = DateTime.Now.ToLongTimeString();
            int tot = iTunes.LibraryPlaylist.Tracks.Count;
            for (int i = 1; i <= tot; i++)
            {
                IITTrack track = ((IITTrack)iTunes.LibraryPlaylist.Tracks[i]);

                track.Name = RSSFor(track.Name);

                Console.WriteLine("Scanning: " + i + "/" + tot);
            }
        }
        static string RSSFor(string text)
        {
            if (text.ToUpper().EndsWith(theSuffix))
                return text.Remove(text.Length - theSuffix.Length);
            else
                return text;
        }
    }
}
