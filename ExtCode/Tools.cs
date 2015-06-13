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
using System.Windows.Media.Effects;

namespace DataManger
{
    public enum AppsModules
    {
        Gallery,
        PDFList,
        Video,
        Maps,
        Puzzle,
        PDFFlip,
        WebBrowser,
        Voting
    }

    public enum ListType
    {
        Folder,
        PDF,
        Video,
        Audio,
        Image,
        Maps,
        Voting,
        None
    }

    class FileMgr
    {
        public static string[] GetFileFormats(ListType type)
        {
            string[] Formats;

            switch (type)
            {
                case ListType.PDF:
                    Formats = new string[] { "*.pdf" };
                    break;
                case ListType.Video:
                    Formats = new string[] { "*.avi", "*.wmv", "*.flv" };
                    break;
                default:
                    Formats = new string[] { "*.*" };
                    break;
            }

            return Formats;
        }
    }

    class ImgManger
    {
        public static Image LoadImageFromFile(string File, string FilePath, int ImWidth, int ImHeight, string Name)
        {
            Image NewImg = new Image();
            BitmapImage scr = new BitmapImage();

            scr.BeginInit();
            scr.UriSource = new Uri(FilePath + File, UriKind.Relative);
            scr.CacheOption = BitmapCacheOption.OnLoad; //Mantenerlo en la memoria
            scr.EndInit();

            NewImg.Source = scr;
            if(ImWidth != 0)
                NewImg.Width = ImWidth;
            if(ImHeight != 0)
                NewImg.Height = ImHeight;
            NewImg.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //NewImg.Stretch = Stretch.Uniform;

            if (Name == "")
            {
                string[] Strsplit = File.Split('.');            //Remove Extention
                string[] SecStrsplit = Strsplit[0].Split('_');  //Remove Numbers

                if (SecStrsplit.Length > 1)
                    NewImg.Name = SecStrsplit[1];
                else
                    NewImg.Name = SecStrsplit[0];
            }
            else
                NewImg.Name = Name;

            return NewImg;
        }

        public static ImageBrush LoadImageFromFile(string File, string FilePath)
        {
            ImageBrush NewImg = new ImageBrush();
            BitmapImage scr = new BitmapImage();

            scr.BeginInit();
            scr.UriSource = new Uri(FilePath + File, UriKind.Relative);
            //scr.CacheOption = BitmapCacheOption.OnLoad; //Mantenerlo en la memoria
            scr.EndInit();

            NewImg.ImageSource = scr;
            NewImg.Stretch = Stretch.Fill;

            return NewImg;
        }
    }

    class GUIMgr
    {
        public static DropShadowBitmapEffect ShadowEffect()
        {
            //Create Shadow Effect
            DropShadowBitmapEffect ShadowEffect = new DropShadowBitmapEffect();
            ShadowEffect.Color = Colors.Black;
            ShadowEffect.Direction = 320;
            ShadowEffect.Opacity = 0.5;
            ShadowEffect.Softness = 1;
            ShadowEffect.ShadowDepth = 15;

            return ShadowEffect;
        }

        public static Border CreateBorderform(string name)
        {
            Border myBorder = new Border();

            myBorder.Name = name;
            myBorder.BitmapEffect = ShadowEffect();
            myBorder.BorderThickness = new Thickness(1);
            myBorder.Margin = new Thickness(20, 15, 20, 10);
            myBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 148, 255));
            //myBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            //myBorder.CornerRadius = new CornerRadius(9);

            //return myBorder;
            return myBorder;
        }

        public static Label CreateLabel(string text)
        {
            return CreateLabel(text, 36, Colors.White);
        }

        public static Label CreateLabel(string text, int FontSize, Color Textcolor)
        {
            Label myLabel = new Label();
            myLabel.Content = text;
            myLabel.FontFamily = new System.Windows.Media.FontFamily("Segoe WP Semibold");
            myLabel.FontSize = FontSize;
            myLabel.FontWeight = FontWeights.Bold;
            myLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            myLabel.HorizontalAlignment = HorizontalAlignment.Center;
            myLabel.Margin = new Thickness(5);
            myLabel.Foreground = new SolidColorBrush(Textcolor);

            return myLabel;
        }
    }
}
