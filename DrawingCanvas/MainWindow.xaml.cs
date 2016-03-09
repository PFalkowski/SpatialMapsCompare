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
            //viewModel.Points.Add(e.GetPosition(canvas));
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            firstPoint = true;
            //canvas.Children.Clear();
            viewModel.Points.Clear();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
