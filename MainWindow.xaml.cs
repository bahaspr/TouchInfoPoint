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

//Extra Namespaces
using System.Configuration;
using System.IO;
using System.Windows.Media.Effects;
using System.Windows.Threading;
//My Namespace
using DataManger;

namespace TouchInfoPoint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Static Element from AppConfig file
        static int TouchAbsVal = Convert.ToInt16(ConfigurationManager.AppSettings.Get("ClickVariation"));
        static string ImgFormat = ConfigurationManager.AppSettings.Get("IconFormat");
        static double PerWidth = Convert.ToDouble(ConfigurationManager.AppSettings.Get("IconWidth"));
        static double PerHeight = Convert.ToDouble(ConfigurationManager.AppSettings.Get("IconHeight"));
        static int ImgMarg = Convert.ToInt16(ConfigurationManager.AppSettings.Get("IconEdge"));
        static double PerImgfromTop = Convert.ToDouble(ConfigurationManager.AppSettings.Get("IconPos"));

        static string URLBrowser = ConfigurationManager.AppSettings.Get("URL_Browser");

        //Element to Control the Icons
        int ImgWidth;
        int ImgHeight;
        int ImgfromTop;
        Image[] MenuImg;
        int NumImg = 1;

        //Flags for Clicks
        bool CanvasClick = false;
        bool ImgClick = false;
        Point altPos;
        Point Entrypt;

        public MainWindow()
        {
            InitializeComponent();

            //Set Timer for the Clock
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(400);
            timer.Tick += ClockUpdate;
            timer.Start();
        }

        public void ClockUpdate(object sender, EventArgs ex)
        {
            Lbl_Time.Content = DateTime.Now.ToString("dd/MM  HH:mm:ss");
        }

        //Static Functions
        public void AppLauncher(AppsModules ApptoLaunch, string AppPath)
        {
            ListType ListType;
            
            switch (ApptoLaunch)
            {
                case AppsModules.Gallery:
                    ListType = DataManger.ListType.Folder;
                    break;
                case AppsModules.PDFFlip:
                case AppsModules.PDFList:
                    ListType = DataManger.ListType.PDF;
                    break;
                case AppsModules.Video:
                    ListType = DataManger.ListType.Video;
                    break;
                case AppsModules.Maps:
                    ListType = DataManger.ListType.Maps;
                    break;
                default:
                    ListType = DataManger.ListType.None;
                    break;
            }

            ListMenu ListForm = new ListMenu(AppPath, ApptoLaunch, ListType);
            if ((ListForm.DirCount != 1) && (ListType != DataManger.ListType.None))
                ListForm.Show();
            else
                ListForm.Launch();
        }

        /************* Canvas CallBaks ***************/

        public void Window_Loaded(object sender, RoutedEventArgs ex)
        {
            //Set-Get Windows Parameters
            this.WindowState = System.Windows.WindowState.Maximized;
            double ScreenW = System.Windows.SystemParameters.PrimaryScreenWidth;
            double ScreenH = System.Windows.SystemParameters.PrimaryScreenHeight;

            //Set the Icons size
            ImgWidth = Convert.ToInt16(ScreenW * PerWidth);
            ImgHeight = Convert.ToInt16(ScreenH * PerHeight);
            if(ImgFormat == "Horizontal")
                ImgfromTop = Convert.ToInt16(PerImgfromTop * ScreenH);
            else
                ImgfromTop = Convert.ToInt16(PerImgfromTop * ScreenW);

            //Set Canvas Property
            Canvas1.Height = ScreenH;
            Canvas1.Width = ScreenW;

            //Get Files from Folder
            string[] FilePaths = Directory.GetFiles(@"MainData\Menus\", "*.png");
            NumImg = FilePaths.Length;

            //Create the Array with the Menu Images
            MenuImg = new Image[NumImg];

            //Load Background from Canvas
            Canvas1.Background = ImgManger.LoadImageFromFile("Background.png", "MainData\\");

            //Load Images for Menu
            for (int x = 0; x < NumImg; x++)
            {
                //Load Image
                MenuImg[x] = ImgManger.LoadImageFromFile(FilePaths[x], "", ImgWidth, ImgHeight, "");

                //Add Effect
                MenuImg[x].BitmapEffect = GUIMgr.ShadowEffect();

                //Set Position
                if (ImgFormat == "Horizontal")
                {
                    Canvas.SetLeft(MenuImg[x], ImgMarg + (x * (ImgWidth + 2 * ImgMarg)));
                    Canvas.SetTop(MenuImg[x], ImgfromTop);
                }
                else
                {
                    Canvas.SetTop(MenuImg[x], ImgMarg + (x * (ImgHeight + 2 * ImgMarg)));
                    Canvas.SetLeft(MenuImg[x], ImgfromTop);
                }
                Canvas1.Children.Add(MenuImg[x]);

                //Add Event Click
                MenuImg[x].MouseDown += new MouseButtonEventHandler(Image_MouseDown);
                MenuImg[x].MouseUp += new MouseButtonEventHandler(Image_MouseUp);
                MenuImg[x].MouseLeave += new MouseEventHandler(Image_MouseLeave);
            }
        }

        private void Canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CanvasClick = true;
        }

        private void Canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            double RealPos = pt.X - altPos.X;
            double RealPosY = pt.Y - altPos.Y;
            //RealPosY = -1;

            if (CanvasClick == true)
            {
                //Validation for the Format
                if (ImgFormat == "Horizontal")
                {
                    if (RealPos != 0)
                    {
                        //Validation from Begin and End the Screen
                        if (((RealPos + Canvas.GetLeft(MenuImg[0])) < ImgMarg) && ((RealPos + Canvas.GetLeft(MenuImg[NumImg - 1])) > (System.Windows.SystemParameters.PrimaryScreenWidth - ImgMarg - ImgWidth)))
                        {
                            for (int x = 0; x < MenuImg.Length; x++)
                            {
                                Canvas.SetLeft(MenuImg[x], RealPos + Canvas.GetLeft(MenuImg[x]));
                            }
                        }
                    }
                }
                else
                {
                    if (RealPosY != 0)
                    {
                        //Validation from Begin and End the Screen
                        if (((RealPosY + Canvas.GetTop(MenuImg[0])) < ImgMarg) && ((RealPosY + Canvas.GetTop(MenuImg[NumImg - 1])) > (System.Windows.SystemParameters.PrimaryScreenHeight - ImgMarg - ImgHeight)))
                        {
                            for (int x = 0; x < MenuImg.Length; x++)
                            {
                                Canvas.SetTop(MenuImg[x], Canvas.GetTop(MenuImg[x]) + RealPosY);
                            }
                        }
                    }
                }
            }

            Lbl_MousePos.Content = string.Format("Mouse {0},{1} [{2} | {3}]", pt.X, pt.Y, altPos.X - pt.X, altPos.Y - pt.Y);

            //Save the currento position
            altPos = pt;
        }

        private void Canvas1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CanvasClick = false;
        }

        private void Canvas1_MouseLeave(object sender, MouseEventArgs e)
        {
            CanvasClick = false;
        }

        /************* Image CallBaks ***************/

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Capture the Mouse Position
            Entrypt = e.GetPosition(this);

            //Cast from Object to Image and change the Width
            Image MenuImg = (Image)sender;
            MenuImg.Width = ImgWidth - 15;
            //Active the Click Flag for Imagen
            ImgClick = true;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Validation from Click Flag
            if (!ImgClick)
                return;

            //Change Flag
            ImgClick = false;

            //Cast from Object to Image and change the Width
            Image MenuImg = (Image)sender;
            MenuImg.Width = ImgWidth;

            //Validation from Movement in X and Y
            Point pt = e.GetPosition(this);
            pt.X -= Entrypt.X;
            pt.Y -= Entrypt.Y;

            if ((Math.Abs(pt.X) > TouchAbsVal) || (Math.Abs(pt.Y) > TouchAbsVal))
                return;

            //Actions for Click Event
            Lbl_Home.Content = MenuImg.Name;

            //Validation for each Icon - App
            switch (MenuImg.Name)
            {
                case "PDFList":
                    AppLauncher(AppsModules.PDFList, @"AppData\PDFList\");
                    break;
                case "Gallery":
                    AppLauncher(AppsModules.Gallery, @"AppData\Gallery\");
                    break;
                case "Video":
                    AppLauncher(AppsModules.Video, @"AppData\Video\");
                    break;
                case "Maps":
                    AppLauncher(AppsModules.Maps, @"AppData\Maps\");
                    break;
                case "Puzzle":
                    AppLauncher(AppsModules.Puzzle, @"AppData\Puzzle\");
                    break;
                case "PDFFlip":
                    AppLauncher(AppsModules.PDFFlip, @"AppData\PDFFlip\");
                    break;
                case "Browser":
                    System.Diagnostics.Process.Start(URLBrowser);
                    break;
                case "Voting":
                    AppLauncher(AppsModules.Voting, @"AppData\Voting\");
                    break;
            }

        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ImgClick == true)
            {
                Image MenuImg = (Image)sender;
                MenuImg.Width = ImgWidth;
                ImgClick = false;
            }
        }

        private void Img_Logo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }
    }
}
