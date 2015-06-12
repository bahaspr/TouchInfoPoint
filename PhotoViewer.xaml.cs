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
    /// Interaction logic for PhotoViewer.xaml
    /// </summary>
    public partial class PhotoViewer : Window
    {
        Photo _photo;

        public PhotoViewer(Photo SelectedPhoto)
        {
            InitializeComponent();

            _photo = SelectedPhoto;

            OutBorder.Width = SystemParameters.PrimaryScreenWidth;
            OutBorder.Height = SystemParameters.PrimaryScreenHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Set the Source File
            myPhoto.Source = _photo.Image;
            Lbl_photo.Content = _photo.Name;

            //Set the Image size
            //CenterBorder.Width = myPhoto.Width = SystemParameters.PrimaryScreenWidth * 0.8;
            myPhoto.Height = SystemParameters.PrimaryScreenHeight * 0.8;
        }

        private void OutBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
