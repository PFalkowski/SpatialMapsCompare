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

namespace DravingCanvas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DravingCanvasViewModel viewModel;
        //private Polygon selectedPolygon { get; set; } = new Polygon { Stroke = Brushes.Gray, StrokeThickness = 1, StrokeDashArray = new DoubleCollection { 3, 3 } };
        //public bool containerEmpty => selectedPolygon.Points.Count == 0;
        public Point startPoint { get; set; }
        private bool firstPoint = true;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new DravingCanvasViewModel();
            DataContext = viewModel;
        }
        

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }


        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //selectedPolygon.Points.Add(e.GetPosition(canvas));
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
            //selectedPolygon = new Polygon { Stroke = Brushes.Gray, StrokeThickness = 1, StrokeDashArray = new DoubleCollection { 3, 3 } };
            firstPoint = true;
            canvas.Children.Clear();
            viewModel.Points.Clear();
            //canvas.Children.Add(selectedPolygon);
        }
    }
}
