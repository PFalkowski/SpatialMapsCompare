using GeoLib;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Prism.Events;
using SpatialMaps;
using System.Collections.Generic;
using System.Linq;

namespace SpatialMapsWpfUi
{
    public class SpatialMapsViewModel : BindableBase, ISpatialMapsViewModel
    {
        public ISpatialMapsModel Model { get; set; }
        public IEventAggregator Events { get; }

        private ObservableCollection<C2DPoint> _leftPoly = new ObservableCollection<C2DPoint>();
        public ObservableCollection<C2DPoint> LeftPoly
        {
            get
            {
                return _leftPoly;
            }
            set
            {
                if (value != _leftPoly)
                {
                    _leftPoly = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<C2DPoint> _rightPoly = new ObservableCollection<C2DPoint>();
        public ObservableCollection<C2DPoint> RightPoly
        {
            get
            {
                return _rightPoly;
            }
            set
            {
                if (value != _rightPoly)
                {
                    _rightPoly = value;
                    OnPropertyChanged();
                }
            }
        }
        private string LeftPolyDefaultName => "Left Polygon";
        private string RightPolyDefaultName => "Right Polygon";

        private string _leftPolyName;
        public string LeftPolyName
        {
            get
            { return _leftPolyName ?? LeftPolyDefaultName; }
            set
            {
                if (_leftPolyName != value)
                {
                    _leftPolyName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _rightPolyName;
        public string RightPolyName
        {
            get
            { return _rightPolyName ?? RightPolyDefaultName; }
            set
            {
                if (_rightPolyName != value)
                {
                    _rightPolyName = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand OpenLeftFileCommand { get; }
        public DelegateCommand OpenRightFileCommand { get; }
        public DelegateCommand SaveLeftFileCommand { get; }
        public DelegateCommand SaveRightFileCommand { get; }
        public DelegateCommand DrawLeftFileCommand { get; }
        public DelegateCommand DrawRightFileCommand { get; }
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand AboutCommand { get; }
        public DelegateCommand SnapToOriginLeftCommand { get; }
        public DelegateCommand SnapToOriginRightCommand { get; }


        public double? LeftPolyArea => Model.GetArea(LeftPoly);
        public double? RightPolyArea => Model.GetArea(RightPoly);
        public double? AreaDifference => LeftPolyArea - RightPolyArea;
        public double? LeftPolyPerimeter => Model.GetPerimeter(LeftPoly);
        public double? RightPolyPerimeter => Model.GetPerimeter(RightPoly);
        public double? PerimeterDifference => LeftPolyPerimeter - RightPolyPerimeter;
        public double? LeftPolyOverlappingArea => Model.GetOverlappingArea(LeftPoly, RightPoly);
        public double? RightPolyOverlappingArea => Model.GetOverlappingArea(RightPoly, LeftPoly);
        public double? LeftPolyNonOverlappingArea => Model.GetNonOverlappingArea(LeftPoly, RightPoly);
        public double? RightPolyNonOverlappingArea => Model.GetNonOverlappingArea(RightPoly, LeftPoly);
        public double? OverlappingAreasSum => LeftPolyOverlappingArea + RightPolyOverlappingArea;
        public double? NonOverlappingAreasSum => LeftPolyNonOverlappingArea + RightPolyNonOverlappingArea;
        public double? ResemblenceIndex
        {
            get
            {
                if (!(LeftPolyOverlappingArea.HasValue && LeftPolyArea.HasValue && RightPolyArea.HasValue)) return null;
                var leftPolyOverlappingArea = LeftPolyOverlappingArea.Value;
                var leftPolyArea = LeftPolyArea.Value;
                var rightPolyArea = RightPolyArea.Value;
                var index = leftPolyOverlappingArea / ((leftPolyArea + rightPolyArea) / 2) * 100;
                return Math.Round(index, 1);
            }
        }

        public SpatialMapsViewModel(ISpatialMapsModel model)
        {
            Model = model;
            Events = new EventAggregator();
            OpenLeftFileCommand = new DelegateCommand(OpenLeftFileSafe);
            OpenRightFileCommand = new DelegateCommand(OpenRightFileSafe);
            SaveLeftFileCommand = new DelegateCommand(SaveLeftFileSafe);
            SaveRightFileCommand = new DelegateCommand(SaveRightFileSafe);
            DrawLeftFileCommand = new DelegateCommand(DrawLeftFileSafe);
            DrawRightFileCommand = new DelegateCommand(DrawRightFileSafe);
            SnapToOriginLeftCommand = new DelegateCommand(SnapToOriginLeft);
            SnapToOriginRightCommand = new DelegateCommand(SnapToOriginRight);
            RefreshCommand = new DelegateCommand(Refresh);
            AboutCommand = new DelegateCommand(About);
        }

        private void SnapToOriginRight()
        {
            try
            {
                if (RightPoly?.Count > 0)
                {
                    Model.SnapToOriginInPlace(RightPoly);
                    Refresh();
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentOutOfRangeException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        private void SnapToOriginLeft()
        {
            try
            {
                if (LeftPoly?.Count > 0)
                {
                    Model.SnapToOriginInPlace(LeftPoly);
                    Refresh();
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentOutOfRangeException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        private void About()
        {
            Model.InputOutputService.PrintToScreen(Properties.Settings.Default.AboutString, "About", MessageSeverity.None);
        }

        public void Refresh()
        {
            var refreshEvents = Events.GetEvent<RefreshEvent>();
            refreshEvents?.Publish(true);

            OnPropertyChanged(nameof(LeftPolyArea));
            OnPropertyChanged(nameof(LeftPolyPerimeter));
            OnPropertyChanged(nameof(AreaDifference));
            OnPropertyChanged(nameof(RightPolyArea));
            OnPropertyChanged(nameof(RightPolyPerimeter));
            OnPropertyChanged(nameof(PerimeterDifference));
            OnPropertyChanged(nameof(LeftPolyOverlappingArea));
            OnPropertyChanged(nameof(LeftPolyNonOverlappingArea));
            OnPropertyChanged(nameof(RightPolyOverlappingArea));
            OnPropertyChanged(nameof(RightPolyNonOverlappingArea));
            OnPropertyChanged(nameof(OverlappingAreasSum));
            OnPropertyChanged(nameof(NonOverlappingAreasSum));
            OnPropertyChanged(nameof(ResemblenceIndex));
        }

        private void OpenLeftFileSafe()
        {
            try
            {
                OpenLeftFile();
                Refresh();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        private void OpenRightFileSafe()
        {
            try
            {
                OpenRightFile();
                Refresh();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }


        private void SaveLeftFileSafe()
        {
            try
            {
                Model.WritePolygonToFile(LeftPoly, Model.InputOutputService.GetFileNameForWrite(null, LeftPolyName, Model.FilterString));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        private void SaveRightFileSafe()
        {
            try
            {
                Model.WritePolygonToFile(RightPoly, Model.InputOutputService.GetFileNameForWrite(null, RightPolyName, Model.FilterString));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        public void OpenLeftFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForRead(null, LeftPolyDefaultName, Model.FilterString);
            if (fileName != null)
            {
                var dataLoaded = Model.GetPolygonFromFile(fileName);
                LeftPolyName = dataLoaded.Key;
                RedrawPoly(LeftPoly, dataLoaded.Value);
                Refresh();
            }
        }

        public void OpenRightFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForRead(null, RightPolyDefaultName, Model.FilterString);
            if (fileName != null)
            {
                var dataLoaded = Model.GetPolygonFromFile(fileName);
                RightPolyName = dataLoaded.Key;
                RedrawPoly(RightPoly, dataLoaded.Value);
                Refresh();
            }
        }

        private void RedrawPoly(ObservableCollection<C2DPoint> polygon, IList<C2DPoint> points)
        {
            polygon.Clear();// ugly hack
            foreach (var point in points)
            {
                polygon.Add(point);
            }
        }

        private void DrawLeftFileSafe()//todo move to model
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(LeftPolyName, LeftPoly);
                var dialogResult = canvas.ShowDialog();
                if (dialogResult != null && (bool)dialogResult)
                {
                    RedrawPoly(LeftPoly, canvas.viewModel.Points);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
            Refresh();
        }

        private void DrawRightFileSafe()
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(RightPolyName, RightPoly);
                var dialogResult = canvas.ShowDialog();
                if (dialogResult != null && (bool)dialogResult)
                {
                    RedrawPoly(RightPoly, canvas.viewModel.Points);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
            Refresh();
        }
    }
}
