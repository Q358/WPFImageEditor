using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private Point start_pos = new Point(0, 0);

        // I understand that these values do not dynamically update the UI Element, but they are a temporary measure to allow the application to run
        private int width = 0;
        private int height = 0;

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
            mouseUp = false;
            if (inside)
            {
                Trace.WriteLine("creating", mouseUp.ToString());
                start_pos = e.GetPosition(MainCanvas);

                SolidColorBrush rect_fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                // rect_fill.Color = Color.FromArgb(255, 255, 255, 0);

                Rectangle rect = new()
                {
                    Fill = rect_fill,
                    Width = width,
                    Height = height,
                };
                MainCanvas.Children.Add(rect);
                Canvas.SetLeft(rect, start_pos.X);
                Canvas.SetBottom(rect, start_pos.Y);
            }
        }

        private void MouseReleased(object sender, MouseEventArgs e)
        {
            Trace.WriteLine("ending", mouseUp.ToString());
            mouseUp = true;
        }

        private void EditRectangleSize(object sender, MouseEventArgs e)
        {
            if(inside & !mouseUp)
            {
                Point current_pos = e.GetPosition(MainCanvas);
                width = Math.Abs(Convert.ToInt32(start_pos.X) - Convert.ToInt32(current_pos.X));
                height = Math.Abs(Convert.ToInt32(start_pos.Y) - Convert.ToInt32(current_pos.Y));
                Trace.WriteLine(width.ToString() + " | " + height.ToString() + " | " + current_pos.ToString() + " | " + e.LeftButton);
            }
        }

        private void Save_Image(object sender, RoutedEventArgs e)
        {
            // SaveFileDialog saveFileDialog = new SaveFileDialog();
            // saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // saveFileDialog.Filter = "Image file (*.jpeg)|*.jpg";
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)this.MainImage.ActualWidth, (int)this.MainImage.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(this.MainImage);
            FileStream stream = File.Create(@$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/rectangle_image.jpeg");
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 90;
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
        }
    }
}
