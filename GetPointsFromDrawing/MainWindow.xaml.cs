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

namespace DrawingCanvas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DrawingCanvasViewModel viewModel { get; set; }
        private Point startPoint { get; set; }
        private bool firstPoint = true;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new DrawingCanvasViewModel();
            DataContext = viewModel;
        }    
        

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (firstPoint)
            {
                startPoint = e.GetPosition(canvas);
                firstPoint = false;
                viewModel.Points.Add(new GeoLib.C2DPoint(startPoint.X, startPoint.Y));
                return;
            }
            var pathTravelled = new Line();
            pathTravelled.Stroke = Brushes.Black;
            pathTravelled.StrokeThickness = 2;
            var finishingPoint = e.GetPosition(canvas);
            pathTravelled.X1 = startPoint.X;
            pathTravelled.Y1 = startPoint.Y;
            pathTravelled.X2 = finishingPoint.X;
            pathTravelled.Y2 = finishingPoint.Y;
            canvas.Children.Add(pathTravelled);
            startPoint = e.GetPosition(canvas);
            viewModel.Points.Add(new GeoLib.C2DPoint(finishingPoint.X, finishingPoint.Y));
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            firstPoint = true;
            canvas.Children.Clear();
            viewModel.Points.Clear();
        }
    }
}
