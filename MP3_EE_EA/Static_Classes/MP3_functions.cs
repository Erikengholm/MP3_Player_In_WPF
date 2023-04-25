using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MP3_EE_EA.Static_Classes
{
    public class MP3_functions
    {
        public static string TimeToString(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            if (time.Hours == 0)
            {
                return time.ToString(@"mm\:ss");
            }
            else
            {
                return time.ToString(@"hh\:mm\:ss");
            }
        }

        public static string Get_Mp3_Folder()
        {
            Directory.GetCurrentDirectory();

            return "m";
        }

    }

   
}
