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
        public static void WelcomeAIT()
        {
            Console.Clear();
            Console.WriteLine("Performing AIT (Artwork in Tag)");
            Console.WriteLine(" - Sticks your artwork into the id3tag.\r\n");
        }
        public static void PerformAIC()
        {
            Console.Title = "Performing AIT (Artwork in Tag)";
            WelcomeCPD();
            int KeySource = PickSource();
            WelcomeCPD();
            int KeyPlayList = PickPlaylist(KeySource);
            //=============================================================================
            //
            WelcomeCPD();
            if (!PickDirectory())
            {
                Console.WriteLine("\r\nOperation Cancelled, Press any key to continue.");
                Console.ReadKey(true);
                return;
            }
            //=============================================================================
            //
            WelcomeCPD();
            PickOverWrite();
            Console.WriteLine("Now Copying PlayList:");
            //try
            //{
                int tot = iTunes.Sources[KeySource].Playlists[KeyPlayList].Tracks.Count;
                for (int i = 1; i <= tot; i++)
                {
                    IITTrack Track = ((IITTrack)iTunes.Sources[KeySource].Playlists[KeyPlayList].Tracks[i]);
                    if (Track.Kind == ITTrackKind.ITTrackKindFile)
                    {
                        IITFileOrCDTrack TrackFile = (IITFileOrCDTrack)Track;
                        AITfor(TrackFile);
                    }
                    else
                    {
                        Console.WriteLine("TrackKind FAILED With: " + Track.Kind);
                        Console.WriteLine("FAILED [" + i.ToString() + "/" + tot.ToString() + "]");
                    }
                }
            //}
            //catch( Exception e)
            //{
            //    MessageBox.Show("Something went wrong with copying the files!\r\n"+e.Message);
            //}
            //=============================================================================
            //
            Console.WriteLine("\r\nTask Finished, Press any key to continue.");
            Console.ReadKey(true);
            
        }
        static void AICFor(ref IITFileOrCDTrack TrackFile)
        {
            // TrackFile.Artwork = TrackFile.Artwork;
        }
    }
}
