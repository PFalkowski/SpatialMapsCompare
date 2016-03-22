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
using System.Windows.Media;

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
                _leftPoly = value;
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
                _rightPoly = value;
            }
        }
        private string LeftPolyDefaultName => "Left Polygon";
        private string RightPolyDefaultName => "Right Polygon";

        private string _leftPolyName;
        public string LeftPolyName
        {
            get
            { return _leftPolyName; }
            set
            {
                if (_leftPolyName != value)
                {
                    _leftPolyName = value;
                    OnPropertyChanged(nameof(LeftPolyName));
                }
            }
        }

        private string _rightPolyName;
        public string RightPolyName
        {
            get
            { return _rightPolyName; }
            set
            {
                if (_rightPolyName != value)
                {
                    _rightPolyName = value;
                    OnPropertyChanged(nameof(RightPolyName));
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
        

        public double? LeftPolyArea => Model.GetArea(LeftPolyName);
        public double? RightPolyArea => Model.GetArea(RightPolyName);
        public double? AreaDifference => LeftPolyArea - RightPolyArea;
        public double? LeftPolyPerimeter => Model.GetPerimeter(LeftPolyName);
        public double? RightPolyPerimeter => Model.GetPerimeter(RightPolyName);
        public double? PerimeterDifference => LeftPolyPerimeter - RightPolyPerimeter;
        public double? LeftPolyOverlappingArea => Model.GetOverlappingArea(LeftPolyName, RightPolyName);
        public double? RightPolyOverlappingArea => Model.GetOverlappingArea(RightPolyName, LeftPolyName);
        public double? LeftPolyNonOverlappingArea => Model.GetNonOverlappingArea(LeftPolyName, RightPolyName);
        public double? RightPolyNonOverlappingArea => Model.GetNonOverlappingArea(RightPolyName, LeftPolyName);
        public double? OverlappingAreasSum => LeftPolyOverlappingArea + RightPolyOverlappingArea;
        public double? NonOverlappingAreasSum => LeftPolyNonOverlappingArea + RightPolyNonOverlappingArea;
        public double? ResemblenceIndex
        {
            get
            {
                if (!(LeftPolyOverlappingArea.HasValue && LeftPolyArea.HasValue && RightPolyArea.HasValue)) return null;
                var index = (LeftPolyOverlappingArea.Value / ((LeftPolyArea.Value + RightPolyArea.Value) / 2)) * 100;
                return Math.Round(index, 1);
            }
        }

        public SpatialMapsViewModel(ISpatialMapsModel model)
        {
            LeftPolyName = LeftPolyDefaultName;
            RightPolyName = RightPolyDefaultName;
            Model = model;
            Events = new EventAggregator();
            OpenLeftFileCommand = new DelegateCommand(openLeftFileSafe);
            OpenRightFileCommand = new DelegateCommand(openRightFileSafe);
            SaveLeftFileCommand = new DelegateCommand(saveLeftFileSafe);
            SaveRightFileCommand = new DelegateCommand(saveRightFileSafe);
            DrawLeftFileCommand = new DelegateCommand(drawLeftFileSafe);
            DrawRightFileCommand = new DelegateCommand(drawRightFileSafe);
            RefreshCommand = new DelegateCommand(Refresh);
            AboutCommand = new DelegateCommand(About);
            Model.AddPolygonToDictionary(LeftPolyName, LeftPoly.ToList());
            Model.AddPolygonToDictionary(RightPolyName, RightPoly.ToList());
        }

        private void About()
        {
            Model.InputOutputService.PrintToScreen(Properties.Settings.Default.AboutString, "About", MessageSeverity.None);
        }

        public void Refresh()
        {
            Model.Update(LeftPolyName, LeftPoly.ToList());
            Model.Update(RightPolyName, RightPoly.ToList());
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

        private void openLeftFileSafe()
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

        private void openRightFileSafe()
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


        private void saveLeftFileSafe()
        {
            try
            {
                Model.WritePolygonToFile(LeftPolyName);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        private void saveRightFileSafe()
        {
            try
            {
                Model.WritePolygonToFile(RightPolyName);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        public void OpenLeftFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForRead(null, LeftPolyName, Model.FilterString);
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
            var fileName = Model.InputOutputService.GetFileNameForRead(null, RightPolyName, Model.FilterString);
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

        private void drawLeftFileSafe()//todo move to model
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(LeftPolyName, LeftPoly);
                var dialogResult = canvas.ShowDialog();
                if ((bool)dialogResult && Model.IsPolygonNew(canvas.viewModel.Points, canvas.viewModel.FileName))
                {
                    RedrawPoly(LeftPoly, canvas.viewModel.Points);
                    var uniqeName = Model.GetUniqueNameForPolygon(canvas.viewModel.FileName);
                    LeftPolyName = uniqeName;
                    Model.AddPolygonToDictionary(uniqeName, canvas.viewModel.Points);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
            Refresh();
        }

        private void drawRightFileSafe()
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(RightPolyName, RightPoly);
                var dialogResult = canvas.ShowDialog();
                if ((bool)dialogResult && Model.IsPolygonNew(canvas.viewModel.Points, canvas.viewModel.FileName))
                {
                    RedrawPoly(RightPoly, canvas.viewModel.Points);
                    var uniqeName = Model.GetUniqueNameForPolygon(canvas.viewModel.FileName);
                    RightPolyName = uniqeName;
                    Model.AddPolygonToDictionary(uniqeName, canvas.viewModel.Points);
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
