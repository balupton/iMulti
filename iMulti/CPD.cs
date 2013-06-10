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
        public static void WelcomeCPD()
        {
            Console.Clear();
            Console.WriteLine("Performing CPD (Copy Playlist to Directory)");
            Console.WriteLine(" - Do not copy protected music.\r\n");
        }
        public static void PerformCPD()
        {
            Console.Title = "Performing CPD (Copy Playlist to Directory)";
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
                        if (TrackFile.Location != null || TrackFile.Location != "")
                        {
                            if (File.Exists(TrackFile.Location))
                            {
                                CPLFor(ref TrackFile);
                                Console.WriteLine("Copied [" + i.ToString() + "/" + tot.ToString() + "]");
                            }
                        }
                        else
                        {
                            Console.WriteLine("File Location FAILED");
                            Console.WriteLine("FAILED [" + i.ToString() + "/" + tot.ToString() + "]");
                        }
                    }
                    else
                    {
                        Console.WriteLine("TrackKind FAILEd With: " + Track.Kind);
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
        static void CPLFor(ref IITFileOrCDTrack TrackFile)
        {
            if (!Directory.Exists(DirLocation + "\\" + TrackFile.Album))
                Directory.CreateDirectory(DirLocation + "\\" + TrackFile.Album);
            string Extension = TrackFile.Location.Substring(TrackFile.Location.LastIndexOf(".") + 1);
            string Location = DirLocation + "\\" + TrackFile.Album + "\\" + TrackFile.TrackNumber.ToString() + " - " + TrackFile.Artist + " - " + TrackFile.Name + "." + Extension;
            File.Copy(TrackFile.Location, Location,OverWrite);
        }
    }
}
