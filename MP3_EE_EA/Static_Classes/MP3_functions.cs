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


            if (0>seconds)
            {
                return "00:00";

            }
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            if (time.Hours == 0)
            {
                return time.ToString(@"mm\:ss");
            }
            else
            {
                return time.ToString(@"hh\:mm\:ss");
            }
        }

    }

   
}
