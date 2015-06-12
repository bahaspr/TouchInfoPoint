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
using System.Windows.Shapes;

//Extra Namespaces
using System.IO;
using DataManger;

namespace TouchInfoPoint
{
    /// <summary>
    /// Interaction logic for ListMenu.xaml
    /// </summary>
    public partial class ListMenu : Window
    {
        string[] DirectoryPaths;
        AppsModules CurrentModul;
        int SelModul;

        Point Entrypt;
        int ListWidth = 481;
        bool ImgClick = false;

        ListType myType;

        public ListMenu(string DataPath, AppsModules Modul, ListType Type, string FileFormat)
        {
            InitializeComponent();

            if (Type == ListType.Folder)
            {
                //Search all Directories
                DirectoryPaths = Directory.GetDirectories(DataPath);
            }
            else if(Type == ListType.Files)
            {
                //Search all Files
                DirectoryPaths = Directory.GetFiles(DataPath, FileFormat);
            }

            //Copy the Option Information
            CurrentModul = Modul;
            SelModul = 0;

            myType = Type;
        }

        //Funtions
        public void Launch()
        {
            //Close Windows
            this.Close();
            
            //Call the App
            switch (CurrentModul)
            {
                case AppsModules.PDFList:
                    PDFList PDFListForm = new PDFList(DirectoryPaths[SelModul]);
                    PDFListForm.Show();
                    break;
                case AppsModules.Gallery:
                    Gallery GalleryForm = new Gallery(DirectoryPaths[SelModul]);
                    GalleryForm.Show();
                    break;
            } 
        }

#region Properties
        public int DirCount
        {
            get
            {
                if (DirectoryPaths != null)
                    return DirectoryPaths.Length;
                else
                    return 0;
            }
        }
        public string SelectedModul
        {
            get
            {
                return DirectoryPaths[SelModul];
            }
        }
#endregion

#region Callbacks

        Border CreateFileBox(string name, int BoxNumber)
        {
            //Create Border
            Border FileBorder = GUIMgr.CreateBorderform("Brd_" + BoxNumber.ToString());

            StackPanel FilePanel = new StackPanel();
            FilePanel.Orientation = Orientation.Horizontal;
            FilePanel.Margin = new Thickness(5);

            //Image for list
            Image NewImg;
            if (myType == ListType.Folder)
                NewImg = ImgManger.LoadImageFromFile("folder.png", "MainData\\", 96, 96, "");
            else
                NewImg = ImgManger.LoadImageFromFile("pdf.png", "MainData\\", 96, 96, "");

            //Create Label
            Label FolderLabel = GUIMgr.CreateLabel(name);

            FileBorder.Child = FilePanel;
            FilePanel.Children.Add(NewImg);
            FilePanel.Children.Add(FolderLabel);

            return FileBorder;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Set Border Background
            OutBorder.Width = SystemParameters.PrimaryScreenWidth;
            OutBorder.Height = SystemParameters.PrimaryScreenHeight;

            //Set Title
            Lbl_List.Content = CurrentModul.ToString();
            
            if (DirectoryPaths.Length < 1)
            {
                //Error Message - NO FILES
                CenterBorder.Child = GUIMgr.CreateLabel("NO FILES IN DIRECTORY", 42, Colors.Black);
            }
            else //Show the List
            {
                //CenterBorder.Background = ImgManger.LoadImageFromFile("Background.png", "MainData\\");
                //Set Border Size
                CenterBorder.Width = SystemParameters.PrimaryScreenWidth * 0.5124;
                CenterBorder.Height = SystemParameters.PrimaryScreenHeight * 0.7812;

                //Load Images for Menu
                for (int x = 0; x < DirectoryPaths.Length; x++)
                {
                    //Get Folder Name
                    string[] strsplit = DirectoryPaths[x].Split('\\');
                    Border ObjFile = CreateFileBox(strsplit[strsplit.Length - 1], x);
                    ButtonStack.Children.Add(ObjFile);

                    ////Add Event Click
                    ObjFile.MouseDown += new MouseButtonEventHandler(Image_MouseDown);
                    ObjFile.MouseUp += new MouseButtonEventHandler(Image_MouseUp);
                    ObjFile.MouseLeave += new MouseEventHandler(Image_MouseLeave);
                }
            }
        }

        /************* Image CallBaks ***************/

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Capture the Mouse Position
            Entrypt = e.GetPosition(this);

            //Cast from Object to Image and change the Width
            Border MenuImg = (Border)sender;
            MenuImg.Width = ListWidth - 15;
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
            Border MenuImg = (Border)sender;
            MenuImg.Width = ListWidth;

            //Validation from Movement in X and Y
            Point pt = e.GetPosition(this);
            pt.X -= Entrypt.X;
            pt.Y -= Entrypt.Y;

            if ((Math.Abs(pt.X) > 5) || (Math.Abs(pt.Y) > 5))
                return;

            //Object selected
            string[] SplitName = MenuImg.Name.Split('_');
            SelModul = Convert.ToInt16(SplitName[1]);
            Launch();
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ImgClick == true)
            {
                Border MenuImg = (Border)sender;
                MenuImg.Width = ListWidth;
                ImgClick = false;
            }
        }

        /************* Border CallBak ***************/

        private void OutBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

#endregion
    }
}
