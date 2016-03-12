using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Data;

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
