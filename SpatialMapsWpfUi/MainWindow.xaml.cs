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

namespace SpatialMapsWpfUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new SpatialMapsViewModel();
            DataContext = viewModel;

            CollectionViewSource leftCollectionViewSource = (CollectionViewSource)(FindResource("LeftCollectionViewSource"));
            leftCollectionViewSource.Source = viewModel.Model.LeftPoly;

            CollectionViewSource rightCollectionViewSource = (CollectionViewSource)(FindResource("RightCollectionViewSource"));
            rightCollectionViewSource.Source = viewModel.Model.RightPoly;
        }
    }
}
