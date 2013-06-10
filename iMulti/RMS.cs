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
        public static void PerformRMS()
        {
            Console.Clear();
            Console.WriteLine("Performing RMS (Remove Missing Songs)");
            Console.Title = "Performing RMS (Remove Missing Songs)";
            Console.WriteLine("Scanning will now start.\r\n");
            string MissingSongs = "";
            string MissingSong;
            int tot = iTunes.LibraryPlaylist.Tracks.Count;
            for (int i = 1; i <= tot; i++)
            {
                if (iTunes.LibraryPlaylist.Tracks[i].Kind == ITTrackKind.ITTrackKindFile)
                {
                    IITFileOrCDTrack file = (IITFileOrCDTrack)iTunes.LibraryPlaylist.Tracks[i];
                    if (file.Location == null || file.Location == "")
                    {
                        MissingSong = "Removed: " + iTunes.LibraryPlaylist.Tracks[i].Artist + " - " + iTunes.LibraryPlaylist.Tracks[i].Name;
                        MissingSongs += MissingSong + "\r\n";
                        Console.WriteLine(MissingSong);
                        iTunes.LibraryPlaylist.Tracks[i].Delete();
                        i--;
                        tot--;
                    }
                }
                Console.WriteLine("Scanning: " + i + "/" + tot);
            }
            Console.Clear();
            Console.WriteLine("Performing RMS (Remove Missing Songs)");
            Console.WriteLine("Here is a List of Removed Songs;\r\n");
            Console.WriteLine(MissingSongs);
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(false);
        }
    }
}
