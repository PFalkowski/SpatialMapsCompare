using GeoLib;
using Microsoft.Practices.Unity;
using SpatialMaps;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpatialMapsWpfUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUnityContainer ioc;
        private ISpatialMapsViewModel viewModel;
        //private SpatialMapsViewModel viewModel;

        public MainWindow()
        {
            ioc = new UnityContainer();
            ioc.Bootstrap();
            viewModel = ioc.Resolve<ISpatialMapsViewModel>();
            //viewModel = new SpatialMapsViewModel(new SpatialMapsModel(new DesktopIOService()));
            viewModel.Events.GetEvent<RefreshEvent>().Subscribe(RefreshView, Prism.Events.ThreadOption.UIThread);
            InitializeComponent();
            DataContext = viewModel;

            var leftCollectionViewSource = (CollectionViewSource)(FindResource("LeftCollectionViewSource"));
            leftCollectionViewSource.Source = viewModel.LeftPoly;

            var rightCollectionViewSource = (CollectionViewSource)(FindResource("RightCollectionViewSource"));
            rightCollectionViewSource.Source = viewModel.RightPoly;
        }


        private void RefreshView(bool shouldProceed = true)
        {
            if (shouldProceed)
            {
                ChartingCanvas.Children.Clear();

                if (viewModel.LeftPoly?.Count > 2)
                {
                    var previous = viewModel.LeftPoly[0];
                    for (var i = 1; i < viewModel.LeftPoly.Count; ++i)
                    {
                        var temp = new Line();
                        temp.Stroke = new SolidColorBrush(Properties.Settings.Default.LeftPolygonColor);
                        temp.StrokeThickness = 2;
                        temp.X1 = previous.X;
                        temp.Y1 = previous.Y;
                        temp.X2 = viewModel.LeftPoly[i].X;
                        temp.Y2 = viewModel.LeftPoly[i].Y;
                        previous = viewModel.LeftPoly[i];
                        ChartingCanvas.Children.Add(temp);
                    }
                }
                if (viewModel.RightPoly?.Count > 2)
                {
                    var previous = viewModel.RightPoly[0];
                    for (var i = 1; i < viewModel.RightPoly.Count; ++i)
                    {
                        var temp = new Line();
                        temp.Stroke = new SolidColorBrush(Properties.Settings.Default.RightPolygonColor);
                        temp.StrokeThickness = 2;
                        temp.X1 = previous.X;
                        temp.Y1 = previous.Y;
                        temp.X2 = viewModel.RightPoly[i].X;
                        temp.Y2 = viewModel.RightPoly[i].Y;
                        previous = viewModel.RightPoly[i];
                        ChartingCanvas.Children.Add(temp);
                    }
                }
            }
        }


        private void RightDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            viewModel.Refresh();
        }

        private void LeftDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            viewModel.Refresh();
        }
    }
}
