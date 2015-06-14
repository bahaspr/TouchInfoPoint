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
using System.Windows.Shapes;

namespace TouchInfoPoint
{
    /// <summary>
    /// Interaction logic for VideoViewer.xaml
    /// </summary>
    public partial class VideoViewer : Window
    {
        string VideoSource;
        bool PlayVideo;
        
        public VideoViewer(string VideoPath)
        {
            InitializeComponent();

            VideoSource = VideoPath;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlayVideo = true;

            //Set the Mediaplayer
            MediaVideo.LoadedBehavior = MediaState.Manual;
            MediaVideo.UnloadedBehavior = MediaState.Stop;
            MediaVideo.Source = new Uri(VideoSource, UriKind.Relative);
            MediaVideo.Play();

            Btn_Close.Opacity = 0.2;
        }

        private void MediaVideo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (PlayVideo)
            {
                PlayVideo = false;
                MediaVideo.Pause();
            }
            else
            {
                PlayVideo = true;
                MediaVideo.Play();
            }
        }

        private void Btn_Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MediaVideo.Stop();
            this.Close();
        }

        private void Btn_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Close.Opacity = 1;
        }

        private void Btn_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Close.Opacity = 0.2;
        }

        private void MediaVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
