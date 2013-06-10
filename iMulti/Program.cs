using System;
using System.Collections;
using System.Text;
using iTunesLib;

namespace iMulti
{
    public partial class Program
    {
        public static iTunesAppClass iTunes;
        static void DisplayWelcome()
        {
            Console.Title = "iMulti - Perform multiple actions with your iTunes library";
            Console.WriteLine("iMulti - Perform multiple actions with your iTunes library");
            Console.WriteLine("---");
            Console.WriteLine("Please Select an Action below:");
            Console.WriteLine("1.  DDS (Delete Duplicate Songs) & RMS (Remove Missings Songs)");
            Console.WriteLine("2.  RMS (Remove Missings Songs)");
            Console.WriteLine("3.  ATC (Artist / Title Corrector)");
            Console.WriteLine("4.  NCS (Not Consolidated Songs)");
            Console.WriteLine("5.  RSS (Remove a Suffix from Songs)");
            Console.WriteLine("6.  CPD (Copy Playlist to Directory)");
            Console.WriteLine("7.  CSV (Change Song Values)");
            Console.WriteLine("8.  DCA (Detect Compilation Albums)");
            Console.WriteLine("9.  RED (Remove Empty Directories)");
            Console.WriteLine("10. FNR (Find N' Replace)");
            Console.WriteLine("A.  About");
            Console.WriteLine("Q.  Quit");
        }
        [STAThread]
        static void Main(string[] args)
        {
            iTunes = new iTunesAppClass();
            
            DisplayWelcome();

            string option = "";
            while (option != "q")
            {
                option = Console.ReadLine().ToLower();
                switch (option)
                {
                    case "~":
                        Console.WriteLine("\r\nAction still being finalized.\r\n\r\nPress any key to continue.");
                        Console.ReadKey(false);
                        goto default;
                    case "1":
                        PerformDDS();
                        goto case "2";
                    case "2":
                        PerformRMS();
                        goto default;
                    case "3":
                        PerformATC();
                        goto default;
                    case "4":
                        PerformNCS();
                        goto default;
                    case "5":
                        PerformRSS();
                        goto default;
                    case "6":
                        PerformCPD();
                        goto default;
                    case "7":
                        PerformCSV();
                        goto default;
                    case "8":
                        PerformDCA();
                        goto default;
                    case "9":
                        PerformRED();
                        goto default;
                    case "10":
                        PerformFNR();
                        goto default;
                    case "A":
                    case "a":
                        PerformAbout();
                        goto default;
                    default:
                        Console.Clear();
                        DisplayWelcome();
                        break;
                    case "Q":
                    case "q":
                        break;
                }
            }
            iTunes = null;
        }

        static void PerformAbout()
        {
            Console.Clear();
            Console.WriteLine("About iMulti");
            Console.Title = "About iMulti";

            Console.WriteLine(
                "\r\niMulti is developed by balupton (http://balupton.com)\r\n"+
                "It was originally created for removing duplicated songs from a iTunes library\r\n"+
                " as we were restoring tracks from dvd format, and wanted to save a lot of time.\r\n"+
                "\r\n"+
                "Over time it's gained more actions as i wanted to automate more common tasks that i perform.\r\n"+
                "The (Artist / Title Corrector) would most probably be the action that will make this program big.\r\n"+
                "\r\n"+
                "Anyway, be sure to drop by my website, or drop me a email if you have comments.\r\n"+
                " - Balupton (Benjamin Arthur Lupton)"+
                "\r\n"+
                "\r\n"+
                "Press any key to continue."
            );

            Console.ReadKey(false);

        }

    }
}
