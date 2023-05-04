using MP3_EE_EA.Models;
using MP3_EE_EA.Static_Classes;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MP3_EE_EA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            datagrid_Songs.ItemsSource = Media_Player_Singleton.Instance.SongModels;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Media_Player_Singleton.Instance.SongModels.Select(c => { c.Song_Is_Playing = false; return c; }).ToList();

            Song_Model song_item = (Song_Model)datagrid_Songs.SelectedItem;

            Media_Player_Singleton.Song_Selection_Is_Changed(song_item, End_Time_For_Song, current_Amount_Of_Song, Progress_Slider);

            Pause_MouseLeftButtonDown(PP_Image_Name, null);

        }

        private void Pause_MouseLeftButtonDown(object sender, MouseButtonEventArgs? e)
        {
            if (sender is Image img && datagrid_Songs.SelectedItem is Song_Model song)
            {

                Media_Player_Singleton.Pause_Or_Play(img,song);
                
                if (!Media_Player_Singleton.Instance.Paused)
                {
                    CallToChildThread(Progress_Slider);
                }

            }
        }
        public async void CallToChildThread(Slider progress_Slider)
        {
            await Task.Delay(2000);

            if (Media_Player_Singleton.Instance.mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                progress_Slider.Maximum = Media_Player_Singleton.Instance.mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                progress_Slider.Value = 0;

                while (progress_Slider.Value < progress_Slider.Maximum)
                {
                    await Task.Delay(1);
                    if (!Media_Player_Singleton.Instance.Paused)
                    {
                        TimeSpan FilePostion = Media_Player_Singleton.Instance.mediaPlayer.Position;

                        progress_Slider.Value = FilePostion.TotalSeconds;
                        current_Amount_Of_Song.Content = MP3_functions.TimeToString(FilePostion.TotalSeconds);
                    }

                }
                Media_Player_Singleton.Instance.Select_Next_Song(datagrid_Songs);
            }
        }

        private void Progress_Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            PP_Image_Name.SetResourceReference(Image.SourceProperty, "Play_Button_Image");
            Media_Player_Singleton.Instance.Paused = true;

            Media_Player_Singleton.Instance.mediaPlayer.Pause();
        }

        private void Progress_Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            PP_Image_Name.SetResourceReference(Image.SourceProperty, "Pause_Button_Image");
            Media_Player_Singleton.Instance.Paused = false;

            var value = Progress_Slider.Value;

            Media_Player_Singleton.Instance.mediaPlayer.Position = TimeSpan.FromSeconds(value);

            Media_Player_Singleton.Instance.mediaPlayer.Play();
        }

        private void Foward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Media_Player_Singleton.Instance.Select_Next_Song(datagrid_Songs);
        }
        private void Backward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Media_Player_Singleton.Instance.Go_Backwards(datagrid_Songs);
        }

        private void Volume_Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (sender is Image Volume_Image)
            {
                Media_Player_Singleton.Instance.Volume_Mute_Or_Unmute(Volume_Image_Name, volume_Slider);
            }
        }

        private void Volume_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double>? e)
        {
            if (sender is Slider volume_Slider)
            {
                Media_Player_Singleton.Instance.Update_Volume(Volume_Image_Name, volume_Slider);
            }
        }

        private void Trash_Can_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete the MP3 file?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (sender is Image image && image.DataContext is Song_Model song && System.IO.File.Exists(song.URL))
                {
                    Media_Player_Singleton.Delete_Song(datagrid_Songs, song);
                }
            }
        }

        private void Add_New_File_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            Media_Player_Singleton.Open_MP3_Folder(datagrid_Songs);
        }

        private void Shuffle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image img)
            {
                Media_Player_Singleton.Shuffle_The_MP3(img, datagrid_Songs);

            }
        }
    }
}
