using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using iTunesLib;

namespace iMulti
{
    public partial class Program
    {
        public static void PerformDCA()
        {
            Console.Clear();
            Console.WriteLine("Performing DCA (Detect Compilation Albums)");
            Console.Title = "Performing DCA (Detect Compilation Albums)";
            Console.WriteLine("Detecting of 'Albums' playlist will now start.\r\n");

            int playlist_location = -1;
            int t = iTunes.LibrarySource.Playlists.Count;
            for (int i = 1; i <= t; i++)
            {
                //Console.WriteLine(i + ": " + iTunes.LibrarySource.Playlists[i].Name);
                if (iTunes.LibrarySource.Playlists[i].Name == "Albums")
                {
                    playlist_location = i;
                    break;
                }
            }
            if (playlist_location == -1)
            {
                Console.WriteLine("Please put all your albums in a playlist titled 'Albums' sorted by Album->TrackNumber.");
                Console.ReadKey(false);
                return;
            }
            else
                Console.WriteLine("Playlist 'Albums' found.\r\n");

            Console.WriteLine("Detecting of Compilations will now start.\r\n");

            IITTrackCollection albums = iTunes.LibrarySource.Playlists[playlist_location].Tracks;
            t = albums.Count-1;
            for (int i = 1; i <= t; i++)
            {
                Console.WriteLine("Scanning: " + i + "/" + t);
                IITTrack current_track = albums[i];
                IITTrack next_track = albums[i + 1];
                if (current_track.Album == next_track.Album && current_track.Artist != next_track.Artist)
                {
                    Console.WriteLine("\r\nCompilation Detected: " + current_track.Album);
                    // Re-Wind to start of album
                    while (current_track.Album == next_track.Album)
                    {
                        i--;
                        current_track = albums[i];
                    }

                    // Set current_track to the first item in the album
                    i++;
                    current_track = albums[i];

                    // Now start setting compilation = true for all tracks in the album
                    while (current_track.Album == next_track.Album)
                    {
                        Console.WriteLine("Updating: " + current_track.Name);
                        current_track.Compilation = true;
                        // Get the next track
                        i++;
                        current_track = albums[i];
                    }
                    Console.WriteLine("");
                }
            }
        }
    }
}
