using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using hTunes;
using System.Data;

namespace hTunes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private DataSet musicDataSet = new DataSet();
        private MusicLib musicLib = new MusicLib();

        public MainWindow()
        {
            InitializeComponent();
    
            musicDataSet.ReadXmlSchema("music.xsd");

            musicDataSet.ReadXml("music.xml");

            dataGrid.ItemsSource = musicDataSet.Tables["song"].DefaultView;

            listBox.Items.Add("All Music");

            musicLib.AddImagestoSongs();

            var playList = musicLib.Playlists;
            
            foreach (var item in playList)
            {
                listBox.Items.Add(item);
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string curItem = listBox.SelectedItem.ToString();

            if (curItem == "All Music")
            {
                dataGrid.ItemsSource = musicDataSet.Tables["song"].DefaultView;
            }
            else
            {
                dataGrid.ItemsSource = musicLib.SongsForPlaylist(curItem).DefaultView;
            }
           
        }

       private void removeFromPlaylist()
       {

       }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = dataGrid.SelectedItem as DataRowView;
            if (rowView != null)
            {
                // Extract the song ID from the selected song
                int songId = Convert.ToInt32(rowView.Row.ItemArray[0]);
                musicLib.PlaySong(songId);
            }
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
             // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.FileName = ""; 
            openFileDialog.DefaultExt = "*.wma;*.wav;*mp3";
            openFileDialog.Filter = "Media files|*.mp3;*.m4a;*.wma;*.wav|MP3 (*.mp3)|*.mp3|M4A (*.m4a)|*.m4a|Windows Media Audio (*.wma)|*.wma|Wave files (*.wav)|*.wav|All files|*.*";

            // Show open file dialog box
            bool? result = openFileDialog.ShowDialog();

            // Load the selected song
            if (result == true)
            {
                Song s = GetSongDetails(openFileDialog.FileName);
                musicLib.AddSong(s);
                musicLib.AddSongToPlaylist(s.Id,"All Music");
            }
        }

        private void newPlaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            musicLib.Save();
        }

        private Song GetSongDetails(string filename)
        {
            Song s = null;

            try
            {
                // PM> Install-Package taglib
                // http://stackoverflow.com/questions/1750464/how-to-read-and-write-id3-tags-to-an-mp3-in-c
                TagLib.File file = TagLib.File.Create(filename);

                s = new Song
                {
                    Title = file.Tag.Title,
                    Artist = file.Tag.AlbumArtists.Length > 0 ? file.Tag.AlbumArtists[0] : "",
                    Album = file.Tag.Album,
                    Genre = file.Tag.Genres.Length > 0 ? file.Tag.Genres[0] : "",
                    Length = file.Properties.Duration.Minutes + ":" + file.Properties.Duration.Seconds,
                    Filename = filename
                };

                return s;
            }
            catch (TagLib.UnsupportedFormatException)
            {
                DisplayError("You did not select a valid song file.");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            return s;
        }

        private void DisplayError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "MiniPlayer", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
