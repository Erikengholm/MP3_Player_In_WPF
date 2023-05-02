using MP3_EE_EA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace MP3_EE_EA.Static_Classes
{
    public class List_Helper
    {
        private static readonly Random rng = new();

        public static List<Song_Model> Fill_List()
        {
            var temp = new List<Song_Model>();

            int counter = 0;


            String[] Artist = { "Erik", "JT", "Leone", "Zelda","Sabaton","Caty" };
            String[] Names = { "just relax nature escape", "the evening", "Spread the crash", "calming winter sonata", "sad Tomorrow" };


            while (counter < 100)
            {
                Song_Model Test_Song = new()
                {
                    URL = "Test",
                    Artist = Artist[rng.Next(Artist.Length - 1)],
                    Length = MP3_functions.TimeToString(rng.NextDouble() * rng.Next(200)),
                    Name = Names[rng.Next(Names.Length - 1)]
                };

                counter++;

                temp.Add(Test_Song);
            }



            return temp;
        }

        public static List<Song_Model> Fill_List_From_Folder()
        {
            var temp = new List<Song_Model>();

            var directory = TryGetMp3Folder();
            if (directory != null)
            {

                var fileList = directory.GetFiles();

                foreach (var item in fileList)
                {


                    TagLib.File tagFile = TagLib.File.Create(item.FullName);
                    string[] artist = tagFile.Tag.Performers;
                   
                    var nameSplit = item.Name.Split(';');

                    Song_Model song = new();

                    if(artist.Any())
                    {
                        song.Name = item.Name.Split('.')[0].Replace('_',' ');

                        if (artist.Length> 1)
                        {
                            song.Artist = string.Join(", ", artist);

                        }
                        else
                        {
                            song.Artist = artist[0];
                        }
                    }
                    else if (!artist.Any() && nameSplit.Length>1)
                    {

                        TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagFile.GetTag(TagTypes.Id3v2);

                        string t = nameSplit[1].Split('.')[0].Replace('_', ' '); ;

                        _ = tag.Performers.Prepend(t);
                        tagFile.Save();

                        song.Name = nameSplit[0];
                        song.Artist = nameSplit[1].Split('.')[0];
                    }
                    else
                    {
                        song.Name = nameSplit[0].Split('.')[0].Replace('_', ' ');
                        song.Artist = "Unknown";                        
                    }
                    song.URL = item.FullName;

                    int second_Amount = (int)tagFile.Properties.Duration.TotalSeconds;

                    int second_Amount_2 = (int)tagFile.Properties.Duration.TotalMilliseconds;



                    TimeSpan time = TimeSpan.FromSeconds(second_Amount);

                    //here backslash is must to tell that colon is
                    //not the part of format, it just a character that we want in output
                    if (time.Hours==0)
                    {
                        song.Length = time.ToString(@"mm\:ss");
                    }
                    else
                    {
                        song.Length = time.ToString(@"hh\:mm\:ss");
                    }
                    song.totale_Amount_Of_Seconds = second_Amount;
                    song.totale_Amount_Of_Seconds_2 = second_Amount_2;

                    temp.Add(song);
                }
            }
         

            return temp;
        }
        public static DirectoryInfo? TryGetMp3Folder(string? currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null)
            {
                if (directory.GetDirectories().FirstOrDefault(d => d.Name == "Mp3_Files") is DirectoryInfo di)
                {
                    return di;
                }
                else if(directory.Name == "MP3_EE_EA")
                {

                    directory = directory.GetDirectories().First(d => d.Name == "MP3_EE_EA");

                    return directory.GetDirectories().FirstOrDefault(d => d.Name == "Mp3_Files");

                }
                else
                {
                    directory = directory.Parent;
                }
            }
            return null;
        }



    }
}
