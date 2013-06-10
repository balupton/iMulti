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
        public static void WelcomeCSV()
        {
            Console.Clear();
            Console.WriteLine("Performing CSV (Change Song Values)");
        }
        public static void PerformCSV()
        {
            IITTrack Track = null;
            int KeySource;
            int KeyPlayList;
            int tot;
            //
            Console.Title = "Performing CSV (Change Song Values)";
            WelcomeCSV();
            KeySource = PickSource();
            WelcomeCSV();
            KeyPlayList = PickPlaylist(KeySource);
            //
            tot = iTunes.Sources[KeySource].Playlists[KeyPlayList].Tracks.Count;
            if (tot >= 1)
            {
                Track = ((IITTrack)iTunes.Sources[KeySource].Playlists[KeyPlayList].Tracks[1]);
            }
            //
            WelcomeCSV();
            Console.WriteLine(
"\r\n"+@"Now choose which Object you would like to Modify or type 'q' to quit:
 1. PlayCount.");
            char Key = Console.ReadKey(true).KeyChar;
        }
    }
}