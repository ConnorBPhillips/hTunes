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
                
            }
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void open_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newPlaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            musicLib.Save();
        }
    }
}
