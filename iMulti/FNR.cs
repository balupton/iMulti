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
        public static void WelcomeFNR()
        {
            Console.Clear();
            Console.WriteLine("Performing FNR (Find N' Replace)");
        }
        public static void PerformFNR()
        {
            WelcomeFNR();

            //Console.WriteLine("Correcting will now start.\r\n");
            Console.WriteLine("Give us the value you want to find:");
            string find = Console.ReadLine();
            Console.WriteLine("Give us the value you want to replace it with:");
            string replace = Console.ReadLine();

            int tot = iTunes.LibraryPlaylist.Tracks.Count;
            for (int i = 1; i <= tot; i++)
            {
                IITTrack track = ((IITTrack)iTunes.LibraryPlaylist.Tracks[i]);
                if ( track.Album != null )
                    track.Album = track.Album.Replace(find, replace);
                track.Name = track.Name.Replace(find, replace);
                track.Artist = track.Artist.Replace(find, replace);
                if (track.Composer != null)
                    track.Composer = track.Composer.Replace(find, replace);
                Console.WriteLine("Progress: " + i + "/" + tot);
            }
        }
    }
}