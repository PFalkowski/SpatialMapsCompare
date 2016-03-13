﻿using GeoLib;
using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpatialMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUnityContainer ioc;
        private ISpatialMapsViewModel viewModel;

        public MainWindow()
        {
            ioc = new UnityContainer();
            ioc.Bootstrap();
            viewModel = ioc.Resolve<ISpatialMapsViewModel>();

            InitializeComponent();
            DataContext = viewModel;

            CollectionViewSource leftCollectionViewSource = (CollectionViewSource)(FindResource("LeftCollectionViewSource"));
            leftCollectionViewSource.Source = viewModel.LeftPoly;

            CollectionViewSource rightCollectionViewSource = (CollectionViewSource)(FindResource("RightCollectionViewSource"));
            rightCollectionViewSource.Source = viewModel.RightPoly;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ChartingCanvas.Children.Clear();

            if (viewModel.LeftPoly?.Count > 2)
            {
                C2DPoint previous = viewModel.LeftPoly[0];
                for (int i = 1; i < viewModel.LeftPoly.Count; ++i)
                {
                    Line temp = new Line();
                    temp.Stroke = Brushes.Black;
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
                C2DPoint previous = viewModel.RightPoly[0];
                for (int i = 1; i < viewModel.RightPoly.Count; ++i)
                {
                    Line temp = new Line();
                    temp.Stroke = Brushes.Black;
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
}
