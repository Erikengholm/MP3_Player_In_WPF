using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3_EE_EA.Models
{
    public class Song_Model
    {

        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Length { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;

        public bool Song_Is_Playing { get; set; } = false;

        public int totale_Amount_Of_Seconds { get; set; }

        public int totale_Amount_Of_Seconds_2 { get; set; }


        public Song_Model()
        {
        }

        public Song_Model(string name, string artist, string length, string uRL)
        {
            Name = name;
            Artist = artist;
            Length = length;
            URL = uRL;
        }
    }
}
