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

//Extra Namespaces
using System.IO;
using System.Collections;

//My Namespace
using DataManger;

namespace TouchInfoPoint
{
    /// <summary>
    /// Interaction logic for Gallery.xaml
    /// </summary>
    public partial class Gallery : Window
    {
        IEnumerable PhotoGallery;
        string ImgDirectory;

        TouchPoint altPos;
        double ActualScrollPos = 0;
        
        public Gallery(string DirectoryPath)
        {
            InitializeComponent();

            ImgDirectory = DirectoryPath;
        }

        private System.Collections.IEnumerable CreatePhotos()
        {
            //Create the Photo Collections with the Thumnails of the image
            List<Photo> MyPhotos = new List<Photo>();

            DirectoryInfo _directory = new DirectoryInfo(ImgDirectory);

            foreach (FileInfo f in _directory.GetFiles("*.jpg"))
                MyPhotos.Add(new Photo(f.FullName, f.Name));

            foreach (FileInfo f in _directory.GetFiles("*.png"))
                MyPhotos.Add(new Photo(f.FullName, f.Name));

            foreach (FileInfo f in _directory.GetFiles("*.bmp"))
                MyPhotos.Add(new Photo(f.FullName, f.Name));

            return MyPhotos;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Set-Get Windows Parameters
            this.WindowState = WindowState.Maximized;
            double ScreenW = SystemParameters.PrimaryScreenWidth;
            double ScreenH = SystemParameters.PrimaryScreenHeight;

            //Load Photo Info (Thumbnail and Name)
            PhotoGallery = CreatePhotos();

            this.Background = ImgManger.LoadImageFromFile("Background.png", "MainData\\");

            //Load Rectangles
            double ImgWidth = ScreenW * 0.1362; //186;
            double ImgHeight = ScreenH * 0.1843;

            ListBoxGallery.ItemsSource = PhotoGallery;

            //Set the name of the Gallery
            string[] DataAux = ImgDirectory.Split('\\');
            Lbl_Home.Content = "Gallery: " + DataAux[DataAux.Length - 1];
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ListBoxGallery_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PhotoViewer phWind = new PhotoViewer((Photo)ListBoxGallery.SelectedItem);
            phWind.Show();
        }

        private void ListBoxGallery_TouchDown(object sender, TouchEventArgs e)
        {
            altPos = e.GetTouchPoint(this);
            ActualScrollPos = MyScroll.HorizontalOffset;
        }

        private void ListBoxGallery_TouchMove(object sender, TouchEventArgs e)
        {
            TouchPoint pt = e.GetTouchPoint(this);
            double RealPos = pt.Position.X - altPos.Position.X;

            if (RealPos != 0)
            {
                //Validation from Begin and End the Screen
                MyScroll.ScrollToHorizontalOffset(ActualScrollPos - RealPos);
            }
        }
    }
}
