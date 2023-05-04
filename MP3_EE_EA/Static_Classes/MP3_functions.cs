using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MP3_EE_EA.Static_Classes
{
    /// <summary>
    /// A Static class that hold free floating functions for Mp3 Player
    /// </summary>
    public static class MP3_functions
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

        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

   
}
