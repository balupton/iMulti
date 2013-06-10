using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using iTunesLib;
using System.IO;
using System.Windows.Forms;

namespace iMulti
{
    public partial class Program
    {
        static string DirLocation = "";
        static bool OverWrite = false;
        static System.Windows.Forms.FolderBrowserDialog FolderBD = new FolderBrowserDialog();

        public static int PickSource()
        {
            Console.WriteLine("Avaliable Sources:");
            int tot = iTunes.Sources.Count;
            for (int i = 1; i <= tot; i++)
            {
                Console.WriteLine(i + ": " + iTunes.Sources[i].Name);
            }
            int KeySource = -1;//Something that doesnt exist
            while (KeySource < 1 || KeySource > iTunes.Sources.Count)
            {
                Console.WriteLine("\r\nWhich Source is the Playlist From?");
                // needs digit checking
                KeySource = int.Parse(Console.ReadKey(false).KeyChar.ToString());
            }
            return KeySource;
        }
        public static int PickPlaylist(int KeySource)
        {
            int KeyPlayList = -1;
            int tottracks = -1;
            while (tottracks <= 1)
            {
                if (tottracks == 0)
                    Console.WriteLine("\r\n");
                Console.WriteLine("Avaliable Playlists from [" + iTunes.Sources[KeySource].Name + "]:");
                int tot = iTunes.Sources[KeySource].Playlists.Count;
                for (int i = 1; i <= tot; i++)
                {
                    Console.WriteLine(i + ": " + iTunes.Sources[KeySource].Playlists[i].Name);
                }
                //
                if (tottracks == 0)
                    Console.WriteLine("\r\n! You must choose a PlayList that has some Tracks in it !");
                KeyPlayList = -1;
                //
                while (KeyPlayList < 1 || KeyPlayList > iTunes.Sources[KeySource].Playlists.Count)
                {
                    Console.WriteLine("\r\nWhich PlayList would you like to Copy?");
                    try
                    {
                        KeyPlayList = int.Parse(Console.ReadLine());
                    }
                    catch { }
                }
                tottracks = iTunes.Sources[KeySource].Playlists[KeyPlayList].Tracks.Count;
            }
            return KeyPlayList;
        }
        public static bool PickDirectory()
        {
            FolderBD.SelectedPath = "";
            if (DirLocation != "")
                FolderBD.RootFolder = Environment.SpecialFolder.Recent;
            if (FolderBD.ShowDialog() == DialogResult.OK)
            {
                DirLocation = FolderBD.SelectedPath;
                return true;
            }
            else
                return false;
        }
        public static void PickOverWrite()
        {
            Console.WriteLine("Would you like to OverWrite existing files? (y/n)");
            char Key = ' ';
            while (Key != 'y' && Key != 'n')
            {
                Key = Console.ReadKey(true).KeyChar;
            }
            if (Key == 'y')
                OverWrite = true;
            else
                OverWrite = false;
        }
    }
}
