using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CaseGuard_Rectangle_Assessment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean inside  = false;
        private Boolean mouseUp = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Upload_Image(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                MainImage.Source = new BitmapImage(fileUri);
            }
        }

        private void OnImageEnter(object sender, MouseEventArgs e)
        {
            inside = true;
            Trace.WriteLine("in");
        }
        private void OnImageExit(object sender, MouseEventArgs e)
        {
            inside = false;
            Trace.WriteLine("out");
        }

        private void CreateRectangle(object sender, MouseButtonEventArgs e)
        {
            if (inside)
            {
                Trace.WriteLine("creating");
                Point start_pos = Mouse.GetPosition(MainCanvas);
                SolidColorBrush clr = new SolidColorBrush("#108beb");
                Rectangle rect = new Rectangle()
                {
                    Fill = "Green";
                };
                while (!mouseUp)
                {
                    Point current_pos = Mouse.GetPosition(MainCanvas);
                    rect.Width = Math.Abs(start_pos.X - current_pos.X);
                    rect.Width = Math.Abs(start_pos.Y - current_pos.Y);
                }
            }
        }

        private void MouseReleased(object sender, MouseEventArgs e)
        {
            mouseUp = true;
        }
    }
}
