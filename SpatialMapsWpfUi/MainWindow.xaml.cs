using Microsoft.Practices.Unity;
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

namespace SpatialMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            IUnityContainer ioc = new UnityContainer();
            ioc.Bootstrap();
            var viewModel = ioc.Resolve<ISpatialMapsViewModel>();

            InitializeComponent();
            DataContext = viewModel;

            CollectionViewSource leftCollectionViewSource = (CollectionViewSource)(FindResource("LeftCollectionViewSource"));
            leftCollectionViewSource.Source = viewModel.LeftPoly;

            CollectionViewSource rightCollectionViewSource = (CollectionViewSource)(FindResource("RightCollectionViewSource"));
            rightCollectionViewSource.Source = viewModel.RightPoly;
        }
    }
}
