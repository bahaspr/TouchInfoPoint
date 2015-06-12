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
    /// Interaction logic for PDFList.xaml
    /// </summary>
    public partial class PDFList : Window
    {
        string PDF_Path;
        
        public PDFList(string PDFPath)
        {
            InitializeComponent();

            PDF_Path = PDFPath;
        }
    }
}
