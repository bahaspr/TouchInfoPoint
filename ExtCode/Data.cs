using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Estra Namespaces
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.IO;

namespace TouchInfoPoint
{
    //Class fot the Data type of each photo
    public class Photo
    {
        public Photo(string path, string name)
        {
            _source = new Uri(path);
            _image = BitmapFrame.Create(_source);
            _name = name;

            //Create Thumbnail for png and Bmp images
            if (_image.Thumbnail == null)
            {
                _myThumbnail = new BitmapImage();
                _myThumbnail.BeginInit();
                _myThumbnail.UriSource = _source;
                _myThumbnail.DecodePixelWidth = 200;
                _myThumbnail.DecodePixelHeight = 180;
                _myThumbnail.EndInit();
            }
        }

        private Uri _source;
        public string Source { get { return _source.ToString(); } }

        private BitmapFrame _image;
        public BitmapFrame Image { get { return _image; } }

        private string _name;
        public string Name { get { return _name; } }

        private BitmapImage _myThumbnail;
        public BitmapSource Thumbnail
        {
            get
            {
                if (_image.Thumbnail != null)
                    return _image.Thumbnail;
                else
                    return _myThumbnail;
            }
        }
    }

    public class PhotoCollection : ObservableCollection<Photo>
    {
        public PhotoCollection() { }

        public PhotoCollection(string path) : this(new DirectoryInfo(path)) { }

        public PhotoCollection(DirectoryInfo directory)
        {
            _directory = directory;
            Update();
        }

        public string Path
        {
            set
            {
                _directory = new DirectoryInfo(value);
                Update();
            }
            get { return _directory.FullName; }
        }

        public DirectoryInfo Directory
        {
            set
            {
                _directory = value;
                Update();
            }
            get { return _directory; }
        }
        private void Update()
        {
            this.Clear();
            try
            {
                foreach (FileInfo f in _directory.GetFiles("*.jpg"))
                    Add(new Photo(f.FullName, f.Name));

            }
            catch (DirectoryNotFoundException)
            {
                System.Windows.MessageBox.Show("No Such Directory");
            }
        }

        DirectoryInfo _directory;
    }
}
