using Microsoft.Win32;
using MP3_EE_EA.Models;
using MP3_EE_EA.Static_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagLib;
using TagLib.Ape;

namespace MP3_EE_EA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private bool Paused = true;

        private MediaPlayer mediaPlayer = new();


        public List<Song_Model> SongModels { get; set; } = List_Helper.Fill_List_From_Folder();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            //song_Models.Add(new Song_Model() { Name = "Ryan", Artist = "George", Length = "4:11", URL = "j" });

        }

        private int viewType;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int ViewType
        {
            get { return viewType; }
            set
            {
                viewType = value;
                OnPropertyChanged("ViewType");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SongModels.Select(c => { c.Song_Is_Playing = false; return c; }).ToList();

            Song_Model song_item = (Song_Model)datagrid_Songs.SelectedItem;

            End_Time_For_Song.Content = song_item.Length;
            current_Amount_Of_Song.Content = MP3_functions.TimeToString(0);


            Progress_Slider.Value = 0;

            mediaPlayer.Stop();
            Paused = true;
            Pause_MouseLeftButtonDown(PP_Image_Name, null);

        }

        private void Pause_MouseLeftButtonDown(object sender, MouseButtonEventArgs? e)
        {
            if (sender is Image img && datagrid_Songs.SelectedItem is Song_Model song)
            {

                if (Paused)
                {
                    img.SetResourceReference(Image.SourceProperty, "Pause_Button_Image");
                    Paused = false;

                    if (!song.Song_Is_Playing)
                    {

                        SongModels.Select(songSelector => songSelector.Song_Is_Playing = false);

                        song.Song_Is_Playing = true;

                        mediaPlayer.Open(new Uri(song.URL));
                        CallToChildThread(Progress_Slider, song.totale_Amount_Of_Seconds);


                    }
                    mediaPlayer.Play();

                }
                else
                {
                    img.SetResourceReference(Image.SourceProperty, "Play_Button_Image");
                    Paused = true;

                    mediaPlayer.Pause();
                }
            }
        }
        public async void CallToChildThread(Slider progress_Slider, int maxSeconds)
        {

            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                progress_Slider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                progress_Slider.Value = 0;

                while (progress_Slider.Value < progress_Slider.Maximum)
                {
                    await Task.Delay(1);
                    if (!Paused)
                    {
                        TimeSpan FilePostion = mediaPlayer.Position;

                        progress_Slider.Value = FilePostion.TotalSeconds;
                        current_Amount_Of_Song.Content = MP3_functions.TimeToString(FilePostion.TotalSeconds);
                    }

                }

                Select_Next_Song();
            }
            else
            {
                await Task.Delay(200);
                CallToChildThread(Progress_Slider, maxSeconds);
            }


        }

        public void Select_Next_Song()
        {

            var The_Selected_Song = datagrid_Songs.SelectedItem;

            var index_Of_Song = datagrid_Songs.Items.IndexOf(The_Selected_Song);


            if ((datagrid_Songs.Items.Count-1)>= (index_Of_Song+1))
            {
                datagrid_Songs.SelectedItem = datagrid_Songs.Items[index_Of_Song + 1];


            }
            else if (datagrid_Songs.Items[0] != null)
            {
                datagrid_Songs.SelectedItem = datagrid_Songs.Items[0];
            }

        }

        private void Progress_Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            PP_Image_Name.SetResourceReference(Image.SourceProperty, "Play_Button_Image");
            Paused = true;

            mediaPlayer.Pause();
        }

        private void Progress_Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            PP_Image_Name.SetResourceReference(Image.SourceProperty, "Pause_Button_Image");
            Paused = false;

            var value = Progress_Slider.Value;

            mediaPlayer.Position = TimeSpan.FromSeconds(value);

            mediaPlayer.Play();
        }

        private void Foward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Select_Next_Song();
        }
        private void Backward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var The_Selected_Song = datagrid_Songs.SelectedItem;

            var index_Of_Song = datagrid_Songs.Items.IndexOf(The_Selected_Song);


            if (index_Of_Song - 1 <0)
            {
                datagrid_Songs.SelectedItem = datagrid_Songs.Items[0];


            }
            else
            {
                datagrid_Songs.SelectedItem = datagrid_Songs.Items[index_Of_Song - 1];
            }
        }

        private void Volume_Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (sender is Image Volume_Image)
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

                    Volume_Slider_ValueChanged(volume_Slider,null);
                }
                else
                {
                    Volume_Image.Tag = "Muted;"+ volume_Slider.Value;
                    volume_Slider.Value = 0;
                    Volume_Slider_ValueChanged(volume_Slider, null);
                }
            }
        }

        private void Volume_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double>? e)
        {
            if (sender is Slider volume_Slider)
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

                mediaPlayer.Volume = volume_Slider.Value/100;
            }
        }

        private void Trash_Can_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete the MP3 file?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //do no stuff
            }
            
        }

        private void Add_New_File_MouseLeftButtonUp(object sender, RoutedEventArgs e)
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
        }
    }
}
