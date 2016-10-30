﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();

            DataSet musicDataSet = new DataSet();
            MusicLib music = new MusicLib();

            musicDataSet.ReadXmlSchema("music.xsd");

            musicDataSet.ReadXml("music.xml");

            dataGrid.ItemsSource = musicDataSet.Tables["song"].DefaultView;
            listBox.Items.Add("All Music");

            var playlist = music.Playlists;
            foreach ( var playlists in playlist)
            {
                listBox.Items.Add(playlists);
            }
        }
    }
}
