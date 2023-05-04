using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3_EE_EA.Models
{
    /// <summary>
    /// Model class for how Mp3 songs should be viewed and handled
    /// </summary>
    ///        

    public class Song_Model
    {
        /// <summary>
        /// The Name of the Song
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The Name of the Artist or Band who is playing the song
        /// </summary>
        public string Artist { get; set; } = string.Empty;
        /// <summary>
        /// A string that says how long the song is in format hh:mm:ss
        /// </summary>
        public string Length { get; set; } = string.Empty;
        /// <summary>
        /// A string that says where the MP3 file is
        /// </summary>
        public string URL { get; set; } = string.Empty;
        /// <summary>
        /// Tells whether the song is being played or not
        /// </summary>
        /// //this is here to ensure if you press tha same song twice the song don't restart
        public bool Song_Is_Playing { get; set; } = false;

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
