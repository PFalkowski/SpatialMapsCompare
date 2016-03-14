using GeoLib;
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

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new DrawingCanvasViewModel();
            DataContext = viewModel;
        }
        public MainWindow(IList<C2DPoint> points)
        {
            InitializeComponent();
            viewModel = new DrawingCanvasViewModel();
            DataContext = viewModel;
            drawPathFromPoints(points);
        }

        public void drawPathFromPoints(IList<C2DPoint> points)
        {
            C2DPoint previous = null;
            if (points?.Count < 2) return;
            if (viewModel.IsFirstPoint && viewModel.StartingPoint != null)
            {
                previous = viewModel.StartingPoint;
            }
            else
            {
                previous = points[0];
            }
            for (int i = 1; i < points.Count; ++i)
            {
                Line temp = new Line();
                temp.Stroke = Brushes.Black;
                temp.StrokeThickness = 2;
                temp.X1 = previous.X;
                temp.Y1 = previous.Y;
                temp.X2 = points[i].X;
                temp.Y2 = points[i].Y;
                previous = points[i];
                canvas.Children.Add(temp);
            }
            viewModel.Points = points.ToList();
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (viewModel.IsFirstPoint)
            {
                var tempPoint1 = e.GetPosition(canvas);
                viewModel.StartingPoint = new C2DPoint(tempPoint1.X, tempPoint1.Y);
                viewModel.IsFirstPoint = false;
                viewModel.Points.Add(new C2DPoint(viewModel.StartingPoint.X, viewModel.StartingPoint.Y));
                return;
            }
            var pathTravelled = new Line();
            pathTravelled.Stroke = Brushes.Black;
            pathTravelled.StrokeThickness = 2;
            var finishingPoint = e.GetPosition(canvas);
            pathTravelled.X1 = viewModel.StartingPoint.X;
            pathTravelled.Y1 = viewModel.StartingPoint.Y;
            pathTravelled.X2 = finishingPoint.X;
            pathTravelled.Y2 = finishingPoint.Y;
            canvas.Children.Add(pathTravelled);
            var tempPoint2 = e.GetPosition(canvas);
            viewModel.StartingPoint = new C2DPoint(tempPoint2.X, tempPoint2.Y);
            viewModel.Points.Add(new GeoLib.C2DPoint(finishingPoint.X, finishingPoint.Y));
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.IsFirstPoint = true;
            canvas.Children.Clear();
            viewModel.Points.Clear();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
