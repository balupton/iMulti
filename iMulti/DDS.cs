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
        static string UserGenre = "Other";
        public static void PerformDDS()
        {
            Console.Clear();
            Console.WriteLine("Performing DDS (Delete Duplicated Songs)");
            Console.Title = "Performing DDS (Delete Duplicated Songs)";

            int dupi = -1;
            int tot = iTunes.LibrarySource.Playlists.Count;
            for (int i = 1; i <= tot; i++)
            {
                //Console.WriteLine(i + ": " + iTunes.LibrarySource.Playlists[i].Name);
                if (iTunes.LibrarySource.Playlists[i].Name == "Duplicates")
                {
                    dupi = i;
                    break;
                }
            }
            if (dupi == -1)
            {
                Console.WriteLine("Please put your duplicate files in the playlist 'Duplicates'.");
                Console.ReadKey(false);
                return;
            }
            else
                Console.WriteLine("Playlist 'Duplicates' found.\r\n");

            UserGenre = "";
            while (UserGenre == "")
            {
                Console.WriteLine("Type your word for ('Other'/English) ('Autre'/French):");
                UserGenre = Console.ReadLine();
            }
            Console.WriteLine("\r\nNow Scanning Songs.\r\n");

            //Compare Tracks 
            ArrayList tracks = new ArrayList();
            tracks.Add(iTunes.LibrarySource.Playlists[dupi].Tracks[1]);
            Console.WriteLine("Scanning: 1/" + iTunes.LibrarySource.Playlists[dupi].Tracks.Count);

            tot = iTunes.LibrarySource.Playlists[dupi].Tracks.Count;
            for (int i = 2; i <= tot; i++)
            {
                if (iTunes.LibrarySource.Playlists[dupi].Tracks[i].Name.ToLower()
                    == iTunes.LibrarySource.Playlists[dupi].Tracks[i - 1].Name.ToLower()
                    && iTunes.LibrarySource.Playlists[dupi].Tracks[i].Artist.ToLower()
                    == iTunes.LibrarySource.Playlists[dupi].Tracks[i - 1].Artist.ToLower())

                    tracks.Add(iTunes.LibrarySource.Playlists[dupi].Tracks[i]);
                else
                {
                    int ct = 1;
                    while (tracks.Count > 1)
                    {
                        IITTrack track1 = ((IITTrack)tracks[0]);
                        IITTrack track2 = ((IITTrack)tracks[ct]);
                        if (track1.Duration > track2.Duration - 10 && track1.Duration < track2.Duration + 10)
                        {
                            if (track2.Album != null && (track1.Album == null || track1.Album == ""))
                            {
                                track1.Album = track2.Album;
                                track1.TrackNumber = track2.TrackNumber;
                                track2.TrackCount = track1.TrackCount;
                            }
                            if (track1.Album != null && (track2.Album == null || track2.Album == ""))
                            {
                                track2.Album = track1.Album;
                                track2.TrackNumber = track1.TrackNumber;
                                track2.TrackCount = track1.TrackCount;
                            }

                            bool proceed = false;
                            if (track1.Album == null && track2.Album == null)
                                proceed = true;
                            else
                                if (track1.Album.ToLower() == track2.Album.ToLower())
                                    proceed = true;
                            if (proceed)
                            {
                                /*
                                bool BasedOnLocation = false;
                                //=====================
                                if (track1.Kind == ITTrackKind.ITTrackKindFile && track2.Kind == ITTrackKind.ITTrackKindFile)
                                {
                                    IITFileOrCDTrack file1 = (IITFileOrCDTrack)track1;
                                    IITFileOrCDTrack file2 = (IITFileOrCDTrack)track2;
                                    if (file2.Location == null || file2.Location == "")
                                    {
                                        BasedOnLocation = true;
                                        //Keep Track 1
                                        StartDelete(ref track1, ref track2);
                                        tot--;
                                        i--;
                                        tracks.RemoveAt(1);
                                    }
                                    else
                                    {
                                        BasedOnLocation = true;
                                        //Keep Track 2
                                        StartDelete(ref track2, ref track1);
                                        tot--;
                                        i--;
                                        tracks.RemoveAt(0);
                                    }
                                }

                                if (!BasedOnLocation)
                                {
                                 * */
                                    if (track1.BitRate >= track2.BitRate)
                                    {
                                        //Keep Track 1
                                        StartDelete(ref track1, ref track2);
                                        tot--;
                                        i--;
                                        tracks.RemoveAt(1);
                                    }
                                    else
                                    {
                                        //Keep Track 2
                                        StartDelete(ref track2, ref track1);
                                        tot--;
                                        i--;
                                        tracks.RemoveAt(0);
                                    }
                                //}
                            }
                            else
                            {
                                if (ct == tracks.Count - 1)
                                {
                                    tracks.RemoveAt(0);
                                    ct = 1;
                                }
                                else
                                    ct++;
                            }
                        }
                        else
                        {
                            if (ct == tracks.Count - 1)
                            {
                                tracks.RemoveAt(0);
                                ct = 1;
                            }
                            else
                                ct++;
                        }
                    }

                    tracks.Clear();
                    tracks.Add(iTunes.LibrarySource.Playlists[dupi].Tracks[i]);
                }
                Console.WriteLine("Scanning: " + i + "/" + tot);
                //Console.WriteLine(iTunes.LibrarySource.Playlists[dupi].Tracks[i].Album);
            }
        }
        static void StartDelete(ref IITTrack Tkeep, ref IITTrack Tdel)
        {
            int ModDate = DateTime.Compare(Tkeep.ModificationDate, Tdel.ModificationDate);
            if (ModDate >= 0)
            {
                if (Tkeep.Composer != null && Tdel.Composer != null)
                    Tkeep.Composer = (string)CompareTrack(Tkeep.Composer, Tdel.Composer, "");
                else if (Tdel.Composer != null)
                    Tkeep.Composer = Tdel.Composer;

                if (Tkeep.Genre != null && Tdel.Genre != null)
                    Tkeep.Genre = (string)CompareTrack(Tkeep.Genre, Tdel.Genre, UserGenre);
                else if (Tdel.Genre != null)
                    Tkeep.Genre = Tdel.Genre;

                Tkeep.Rating = (int)CompareTrack(Tkeep.Rating, Tdel.Rating, 0);
                Tkeep.Year = (int)CompareTrack(Tkeep.Year, Tdel.Year, 0);
                Tkeep.PlayedCount += Tdel.PlayedCount;
                //Tkeep.PlayedDate    = CompareTrack(Tkeep.PlayedDate, Tdel.PlayedDate);
            }
            else
            {
                if (Tkeep.Composer != null && Tdel.Composer != null)
                    Tkeep.Composer = (string)CompareTrack(Tdel.Composer, Tkeep.Composer, "");
                else if (Tdel.Composer != null)
                    Tkeep.Composer = Tdel.Composer;

                if (Tkeep.Genre != null && Tdel.Genre != null)
                    Tkeep.Genre = (string)CompareTrack(Tdel.Genre, Tkeep.Genre, UserGenre);
                else if (Tdel.Genre != null)
                    Tkeep.Genre = Tdel.Genre;

                Tkeep.Rating = (int)CompareTrack(Tdel.Rating, Tkeep.Rating, 0);
                Tkeep.Compilation = Tdel.Compilation;
                Tkeep.Year = (int)CompareTrack(Tdel.Year, Tkeep.Year, 0);
                Tkeep.PlayedCount += Tdel.PlayedCount;
                //Tdel.PlayedDate   = CompareTrack(Tdel.PlayedDate, Tkeep.PlayedDate);
            }

            if (Tdel.Kind == ITTrackKind.ITTrackKindFile)
            {
                IITFileOrCDTrack file = (IITFileOrCDTrack)Tdel;
                if (file.Location != null || file.Location != "")
                {
                    FileInfo fi = new FileInfo(file.Location);
                    if (fi.Exists)
                        fi.Delete();
                }
            }
            Console.WriteLine("Deleted: " + Tdel.PlayOrderIndex.ToString() + ":" + Tdel.Name);
            Tdel.Delete();
        }
        static object CompareTrack(object newerT, object olderT, object Comparer)
        {
            if (newerT.ToString() == Comparer.ToString())
            {
                if (olderT.ToString() != Comparer.ToString())
                {
                    Console.WriteLine("Replaced: [" + newerT.ToString() + "]\tWith: [" + olderT.ToString() + "]");
                    return olderT;
                }
                else
                    return newerT;
            }
            else
                return newerT;
        }
    }
}
