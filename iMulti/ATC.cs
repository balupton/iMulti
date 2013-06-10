using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using iTunesLib;

using System.Text.RegularExpressions;


namespace iMulti
{
    public partial class Program
    {
        public static void PerformATC()
        {
            Console.Clear();
            Console.WriteLine("Performing ATC (Artist / Title Corrector)");
            Console.Title = "Performing ATC (Artist / Title Corrector)";
            Console.WriteLine(
                "\r\nWhat this function does is correct your Artist and Title information into\r\n"+
                " the following format:\r\n"+
                " Track Name (Name Extension) [Feat./Vs. Artist] [Someone's Remix/Edit/Mix]\r\n\r\n"+
                "To do this, we correct the character cases (E.g. a A), correct brackets, \r\n"+
                " correct misspellings of 'Vs. ' 'Feat. ' and 'Remix', replace round brackets\r\n"+
                " with square brackets for special parts, and move the special parts to the end.\r\n\r\n"+
                "We can also move [Feat. Artist] parts from the Artist tag into the Title tag, \r\n"+
                " if you would like to use this functionality, press the 'y' key.\r\n");

            //Console.WriteLine(ATC_put_square_brackets_last("Somethign (Something) [ASd Remix] [Feat. Someone] (Testing) [Feat. Someone] [Asd Mix]"));
            //Console.WriteLine(ATC_brackets_all("Today is Tomorrow [Feat. Morris] [Official Street Parade Hymn 2005] [Yoko House Remix]"));


            Console.WriteLine("\r\nPress any key to continue.");
            bool feat_in_title = char.ToLower(Console.ReadKey(true).KeyChar) == 'y' ? true : false;

            if (feat_in_title)
                Console.WriteLine("You have chosen to move [Feat. Artist] information into the Title tag.\r\n");

            /*
            Console.WriteLine(
                "\r\nWe can also correct common mispellings of artist names.\r\n"+
                " like [DJ Tiesto] to [Tiësto], or [Sigur Ros] to [Sigur Rós].\r\n"+
                " if you would like to use this functionality, press the 'y' key.\r\n");
            bool correct_artist_names = char.ToLower(Console.ReadKey(true).KeyChar) == 'y' ? true : false;

            if (correct_artist_names)
                Console.WriteLine("You have chosen to correct mispellings of artist names.\r\n");
            */

            Console.WriteLine("Correcting will now start.\r\n");
            int tot = iTunes.LibraryPlaylist.Tracks.Count;


            if (feat_in_title)
            {
                for (int i = 1; i <= tot; i++)
                {
                    IITTrack track = ((IITTrack)iTunes.LibraryPlaylist.Tracks[i]);
                    Console.WriteLine("Correcting: " + i + "/" + tot);
                    ATC_track_altern(track);
                }
            }
            else
            {
                for (int i = 1; i <= tot; i++)
                {
                    IITTrack track = ((IITTrack)iTunes.LibraryPlaylist.Tracks[i]);
                    Console.WriteLine("Correcting: " + i + "/" + tot);
                    ATC_track(track);
                }
            }

        }

        static string ATC_correct_brackets(string text)
        {   // Will correct a the brackets of a given string
            string result = text.Replace("  ", " ").
                Replace('[', '(').Replace(']', ')').
                Replace('{', '(').Replace('}', ')');

            string pattern, replace;

            //Console.WriteLine("0. "+result);

            pattern = @"V(s|ersus)\.?\s";
            replace = @"Vs. ";
            result = Regex.Replace(result, pattern, replace);

            //Console.WriteLine("1. " + result);

            pattern = @"R(mx(\.)?|emix\.)\)?";
            replace = @"Remix)";
            result = Regex.Replace(result, pattern, replace);

            //Console.WriteLine("2. " + result);

            pattern = @"\s?\(?F(t|eat|eaturing)\.?\s([^()-]*)(-)?"; // Adds support for, Feat. sadad - Title
            replace = @" (Feat. $2)";
            result = Regex.Replace(result, pattern, replace);

            //Console.WriteLine("3. " + result);

            result = result.Replace(')', '(').Replace("((", "( (");

            //Console.WriteLine("4. " + result);

            pattern = @"\(([^()]*)\)?"; // Fix any left over brackets (s
            replace = @"($1)";
            result = Regex.Replace(result, pattern, replace);

            //Console.WriteLine("5. " + result);

            pattern = @"(\)\(\s)([^)]*)(\))"; // Fix things like; (I just died)( in your arms)
            replace = @") $2";
            result = Regex.Replace(result, pattern, replace);

            //Console.WriteLine("6. " + result);

            pattern = @"\([\s]*\)"; // Remove empty brackets; ( ) () (   )
            replace = @"";
            result = Regex.Replace(result, pattern, replace);

            //Console.WriteLine("7. " + result);

            pattern = @"\([\s]+"; // Remove any spaces to left of bracket
            replace = @"(";
            result = Regex.Replace(result, pattern, replace);

            //Console.WriteLine("8. " + result);

            pattern = @"[\s]+\)"; // Remove any spaces to right of bracket
            replace = @")";
            result = Regex.Replace(result, pattern, replace).Replace(")(", ") (");

            //Console.WriteLine("9. " + result);

            return result;
        }
        static string ATC_use_square_brackets(string text)
        {
            string result, pattern, replace;
            result = text;

            //Console.WriteLine("10a. " + result);

            pattern = @"\((([^()]*)((Remix|Mix|Edit)|(Feat|Vs)\.\s[^()]*))\)";// @"\((([^()]*)(Remix|Feat|Vs|Mix|[0-9]|Edit)([^()]*))\)";
            replace = @"[$1]";
            result = Regex.Replace(result, pattern, replace);

            //Console.WriteLine("10b. " + result);

            return result;
        }
        static string ATC_put_square_brackets_last(string text)
        {   // Make it so, square brackets go before round brackets
            string result, pattern, replace;
            result = text;

            pattern = @"(\[[^\[\]]*\])\s(\([^()]*\))";
            replace = @"$2 $1";
            while (result != (result = Regex.Replace(result, pattern, replace))) { }

            //Console.WriteLine("11. " + result);

            pattern = @"(\[[^\]]*(Remix|Mix|[0-9]|Edit)\])\s(\[[^\]]*(Feat|Vs)\.\s[^\]]*\])";
            replace = @"$3 $1";
            while (result != (result = Regex.Replace(result, pattern, replace))) { }

            //Console.WriteLine("12. " + result);

            return result;
        }

        static string ATC_brackets_all(string text)
        {   // Perform all the brackets
            return
                ATC_put_square_brackets_last(
                    ATC_use_square_brackets(
                        ATC_correct_brackets(text)
                    )
                )
            ;
        }


        static bool ATC_caps_check(char c)
        {
            switch (c)
            {
                case '.':
                case '(':
                case '{':
                case '[':
                case '-':
                case '/':
                case '^':
                case '&':
                case ' ':
                    return true;
            }
            return false;
        }
        static string ATC_case_corrector(string text)
        {
            int tot = text.Length;
            text = text.ToUpper();

            // Create a regular expression that matches a series of one 
            // or more white spaces.
            //string pattern = @"s/(^.)|(.)/";
            //Console.WriteLine(Regex.Replace(text,pattern,@"\u$1"));

            // Make words that should be lowercase lowercase
            text = text.
                Replace("_", " ").
                Replace("ARE ", "are ").
                Replace("AND ", "and ").
                Replace("OR ", "or ").
                Replace("ON ", "on ").
                Replace("IT ", "it ").
                Replace("IN ", "in ").
                Replace("AT ", "at ").
                Replace("A ", "a ").
                Replace("AS ", "as ").
                Replace("THE ", "the ").
                Replace("IS ", "is ").
                Replace("OF ", "of ").
                Replace("MY ", "my ").
                Replace("THAT ", "that ").
                Replace("FOR ", "for ").
                Replace("TO ", "to ").
                Replace("BY ", "by ").
                Replace("BE ", "be ").
                Replace("FROM ", "from ");

            // Make it so the first char is uppercase, and the 2nd is lowercase
            if (tot == 1)
                return char.ToUpper(text[0]).ToString();
            else if (tot == 2)
                return char.ToUpper(text[0]).ToString() + char.ToLower(text[1]).ToString();
            else
                text = char.ToUpper(text[0]).ToString() + char.ToLower(text[1]).ToString() + text.Substring(2);

            //Console.WriteLine(text);

            for (int i = 2; i < tot; i++)
            {
                if (char.IsLetter(text[i]))
                {   // No point working with a number or symbol

                    if (char.IsUpper(text[i]))
                    {   // We do not want to 'fix' one of the above words
                        if (!ATC_caps_check(text[i - 1]))
                            text = text.Substring(0, i) + char.ToLower(text[i]).ToString() + text.Substring(i + 1);
                    }
                    else if (!char.IsLetterOrDigit(text[i - 2]) && ATC_caps_check(text[i - 1]))
                    {   // We want to fix one of the above if it is something like " & the"
                        text = text.Substring(0, i) + char.ToUpper(text[i]).ToString() + text.Substring(i + 1);
                    }

                }
            }

            if (ATC_caps_check(text[0]))
                text = char.ToUpper(text[0]).ToString() + char.ToUpper(text[1]).ToString() + text.Substring(2);

            return text;
        }


        static void ATC_track(IITTrack track)
        {
            track.Artist =
                ATC_brackets_all(ATC_case_corrector(track.Artist)).Trim();
            track.Name =
                ATC_brackets_all(ATC_case_corrector(track.Name)).Trim();
        }

        static void ATC_track_altern(IITTrack track)
        {
            string artist = ATC_brackets_all(ATC_case_corrector(track.Artist));
            string name = ATC_brackets_all(ATC_case_corrector(track.Name));

            string pattern = @"(\s?\[Feat\.\s[^()]*\])";
            MatchCollection matches = Regex.Matches(artist, pattern);
            int s = matches.Count;
            for (int i = 0; i < s; i++)
            {
                name += matches[i].Value;
            }
            artist = Regex.Replace(artist, pattern, @"");

            artist = artist.Trim();
            name = name.Trim();
            
            /*if (artist != track.Artist)
            {
                Console.WriteLine("\r\nArtist: {" + track.Artist + "}\r\nNew:    {" + artist + "}\r\n");
                Console.ReadKey(true);
                if (name != track.Name)
                {
                    Console.WriteLine("Name: {" + track.Name + "}\r\nNew:  {" + name + "}\r\n");
                    Console.ReadKey(true);
                }
            }*/

            track.Artist = artist;
            track.Name = name;
        }
    }
}
