using Microsoft.Win32;
using MP3_EE_EA.Static_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MP3_EE_EA.Models
{
    public sealed class Media_Player_Singleton
    {
        private static Media_Player_Singleton? instance = null;
        private static readonly object padlock = new();


        public bool Paused = true;

        public readonly MediaPlayer mediaPlayer = new();


        public bool Shuffle = false;

        public int Shuffle_Index { get; set; } = 0;

        public List<int> Shuffle_Song_Order { get; set; } = new List<int>();


        public List<Song_Model> SongModels { get; set; } = List_Helper.Fill_List_From_Folder();

        Media_Player_Singleton()
        {
        }

        public static Media_Player_Singleton Instance
        {
            get
            {
                lock (padlock)
                {
                    instance ??= new Media_Player_Singleton();
                    return instance;
                }
            }
        }

        public static void Song_Selection_Is_Changed(Song_Model Selected_Song, Label End, Label start, Slider slider)
        {
            _ = Instance.SongModels.Select(c => { c.Song_Is_Playing = false; return c; }).ToList();


            if (Selected_Song != null)
            {
                End.Content = Selected_Song.Length;
                start.Content = MP3_functions.TimeToString(0);


                slider.Value = 0;

                Instance.mediaPlayer.Stop();
                Instance.Paused = true;
            }
        }

        public static void Pause_Or_Play(Image img, Song_Model song)
        {


            if (Instance.Paused)
            {
                img.SetResourceReference(Image.SourceProperty, "Pause_Button_Image");
                Instance.Paused = false;

                if (!song.Song_Is_Playing)
                {

                    _ = Instance.SongModels.Select(songSelector => songSelector.Song_Is_Playing = false);

                    song.Song_Is_Playing = true;

                    Instance.mediaPlayer.Open(new Uri(song.URL));


                }
                Instance.mediaPlayer.Play();

            }
            else
            {
                img.SetResourceReference(Image.SourceProperty, "Play_Button_Image");
                Instance.Paused = true;

                Instance.mediaPlayer.Pause();
            }
        }

        public void Select_Next_Song(DataGrid datagrid_Songs)
        {

            var The_Selected_Song = datagrid_Songs.SelectedItem;

            var index_Of_Song = datagrid_Songs.Items.IndexOf(The_Selected_Song);

            if (Shuffle)
            {
                if (datagrid_Songs.Items[Shuffle_Song_Order[Shuffle_Index]] != null)
                {
                    Shuffle_Index++;

                    if (Shuffle_Index > Shuffle_Song_Order.Count - 1)
                    {
                        Shuffle_Index = 0;
                    }

                    datagrid_Songs.SelectedItem = datagrid_Songs.Items[Shuffle_Song_Order[Shuffle_Index]];
                }
                else if (datagrid_Songs.Items[0] != null)
                {
                    datagrid_Songs.SelectedItem = datagrid_Songs.Items[0];
                }
            }
            else
            {
                if ((datagrid_Songs.Items.Count - 1) >= (index_Of_Song + 1))
                {
                    datagrid_Songs.SelectedItem = datagrid_Songs.Items[index_Of_Song + 1];


                }
                else if (datagrid_Songs.Items[0] != null)
                {
                    datagrid_Songs.SelectedItem = datagrid_Songs.Items[0];
                }
            }
        }


        public void Go_Backwards(DataGrid datagrid_Songs)
        {


            if (Shuffle)
            {
                if (datagrid_Songs.Items[Shuffle_Song_Order[Shuffle_Index]] != null)
                {
                    Shuffle_Index--;

                    if (Shuffle_Index < 0)
                    {
                        Shuffle_Index = Shuffle_Song_Order.Count - 1;
                    }

                    datagrid_Songs.SelectedItem = datagrid_Songs.Items[Shuffle_Song_Order[Shuffle_Index]];
                }
                else if (datagrid_Songs.Items[0] != null)
                {
                    datagrid_Songs.SelectedItem = datagrid_Songs.Items[0];
                }
            }
            else
            {
                var The_Selected_Song = datagrid_Songs.SelectedItem;

                var index_Of_Song = datagrid_Songs.Items.IndexOf(The_Selected_Song);

                if (index_Of_Song - 1 < 0)
                {
                    datagrid_Songs.SelectedItem = datagrid_Songs.Items[0];
                }
                else
                {
                    datagrid_Songs.SelectedItem = datagrid_Songs.Items[index_Of_Song - 1];
                }
            }
        }

        public void Update_Volume(Image Volume_Image_Name, Slider volume_Slider)
        {
            if (volume_Slider.Value == 0)
            {
                Volume_Image_Name.SetResourceReference(Image.SourceProperty, "Volume_mute");
            }
            else if (volume_Slider.Value < 25)
            {
                Volume_Image_Name.SetResourceReference(Image.SourceProperty, "Volume_0");
            }
            else if (volume_Slider.Value < 50)
            {
                Volume_Image_Name.SetResourceReference(Image.SourceProperty, "Volume_25");
            }
            else if (volume_Slider.Value < 75)
            {
                Volume_Image_Name.SetResourceReference(Image.SourceProperty, "Volume_50");
            }
            else
            {
                Volume_Image_Name.SetResourceReference(Image.SourceProperty, "Volume_75");
            }

            mediaPlayer.Volume = volume_Slider.Value / 100;
        }

        public void Volume_Mute_Or_Unmute(Image Volume_Image, Slider volume_Slider)
        {

            string? tags = Volume_Image.Tag as string;

            double Old_Volume = 10;

            if (tags is not null)
            {
                var stringArray = tags.Split(";");

                _ = double.TryParse(stringArray.Last(), out Old_Volume);
            }

            if (tags is string tagString && tagString.Contains("Muted"))
            {
                Volume_Image.Tag = "Not_Mute";
                volume_Slider.Value = Old_Volume;

                Update_Volume(Volume_Image, volume_Slider);
            }
            else
            {
                Volume_Image.Tag = "Muted;" + volume_Slider.Value;
                volume_Slider.Value = 0;
                Update_Volume(Volume_Image, volume_Slider);
            }
        }

        public static void Delete_Song(DataGrid datagrid_Songs, Song_Model song)
        {
            System.IO.File.Delete(song.URL);
            Instance.SongModels = List_Helper.Fill_List_From_Folder();
            datagrid_Songs.ItemsSource = null;
            datagrid_Songs.ItemsSource = Instance.SongModels;
            datagrid_Songs.Items.Refresh();

            if (Instance.mediaPlayer.Source == new Uri(song.URL))
            {
                Instance.Select_Next_Song(datagrid_Songs);
            }


        }

        public static void Open_MP3_Folder(DataGrid datagrid_Songs)
        {
            var folder = List_Helper.TryGetMp3Folder();



            OpenFileDialog openFileDialog = new()
            {
                Filter = "mp3 files (*.mp3)|*.mp3",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (folder != null)
            {
                openFileDialog.InitialDirectory = folder.FullName;

            }
            else
            {
                openFileDialog.InitialDirectory = "C:\\";
            }

            if (openFileDialog.ShowDialog() == true)
            {


            }
            Instance.SongModels = List_Helper.Fill_List_From_Folder();
            datagrid_Songs.ItemsSource = null;
            datagrid_Songs.ItemsSource = Instance.SongModels;
            datagrid_Songs.Items.Refresh();
        }

        public static void Shuffle_The_MP3(Image img,DataGrid datagrid_Songs)
        {

            Instance.Shuffle = !Instance.Shuffle;
                if (Instance.Shuffle)
                {
                    Instance.Shuffle_Song_Order = Enumerable.Range(0, Instance.SongModels.Count).ToList();

                    Instance.Shuffle_Song_Order.Shuffle();

                    Instance.Shuffle_Index = datagrid_Songs.Items.IndexOf(datagrid_Songs.SelectedItem);

                    img.SetResourceReference(Image.SourceProperty, "Shuffle_On");
                }
                else
                {
                    img.SetResourceReference(Image.SourceProperty, "Shuffle_Off");
                }
            
        }

    }
}
