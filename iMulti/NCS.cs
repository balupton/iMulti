using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using iTunesLib;
using System.IO;
using System.Diagnostics;

namespace iMulti
{
    public partial class Program
    {
        //NOT CONSOLIDATED SONGS
        public static void PerformNCS()
        {
            Console.Clear();
            Console.WriteLine("Performing NCS (Not Consolidated Songs)");
            Console.Title = "Performing NCS (Not Consolidated Songs)";
            Console.WriteLine("Scanning will now start.\r\n");
            //string dt1 = DateTime.Now.ToLongTimeString();
            int tot = iTunes.LibraryPlaylist.Tracks.Count;
            int ncss = 0;
            //File.Delete("C:\\NCS.txt");
            //StreamWriter sw = File.AppendText("C:\\NCS.txt");
            //sw.AutoFlush = true;
            for (int i = 1; i <= tot; i++)
            {
                if (iTunes.LibraryPlaylist.Tracks[i].Kind == ITTrackKind.ITTrackKindFile)
                {
                    IITFileOrCDTrack file = (IITFileOrCDTrack)iTunes.LibraryPlaylist.Tracks[i];
                    if (file.Location != null)
                        if (!file.Location.Contains("iTunes Music"))
                        {
                            string location = file.Location;
                            if (file.Location.StartsWith(@"C:\Documents and Settings\Amelie\Mes documents\Ma musique\MyiPodBackup\"))
                                location = file.Location.Replace(@"C:\Documents and Settings\Amelie\Mes documents\Ma musique\MyiPodBackup\", @"C:\MusicFolder\");
                            if (file.Location.StartsWith(@"C:\audiograbber\"))
                                location = file.Location.Replace(@"C:\audiograbber\", @"C:\MusicFolder\");

                            string origdir = location.Remove(location.LastIndexOf("\\"))+"\\";
                            if (Directory.Exists(origdir) == false)
                            {
                                string dir = "";
                                while (Directory.Exists(origdir) == false)
                                {
                                    int l = dir.Length;
                                    dir = location.Substring(0, location.IndexOf("\\", l)) + "\\";
                                    Directory.CreateDirectory(dir);
                                }
                            }
                            
                            File.Move(file.Location, location);
                            Console.WriteLine("Moved the song to:");
                            Console.WriteLine(location);
                            ncss++;
                        }
                }
                Console.WriteLine("Scanning: " + i + "/" + tot);
            }
            //sw.Close();
            //Process.Start("C:\\NCS.txt");
            string[] files = Directory.GetFiles(@"C:\MusicFolder", "*", SearchOption.AllDirectories);
            tot = files.Length;
            for (int a = 0; a < tot; a++)
            {
                string newname = files[a];
                newname = newname.Remove(newname.LastIndexOf("\\")) + "\\";
                newname += a.ToString() + "algkjfovuoa.mp3";
                //Console.WriteLine("Changed;");
                //Console.WriteLine(files[a]);
                //Console.WriteLine(newname);
                File.Move(files[a], newname);
                Console.WriteLine("Correcting Names for Import... (" + a.ToString() + "/" + tot);
            }
            Console.WriteLine("Total NCSs: " + ncss.ToString());
            Console.WriteLine(@"Now add the folder 'C:\MusicFolder\' to ur itunes library and then perform DDS, or Sync Ratings with your ipod, then do RMS" );
            Console.ReadKey(false);
        }
    }
}
