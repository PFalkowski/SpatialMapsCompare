using GeoLib;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Prism.Events;
using SpatialMaps;
using System.Collections.Generic;

namespace SpatialMapsWpfUi
{
    public class SpatialMapsViewModel : BindableBase, ISpatialMapsViewModel
    {
        public ISpatialMapsModel Model { get; set; }
        public IEventAggregator Events { get; }
        public ObservableCollection<C2DPoint> LeftPoly { get; set; } = new ObservableCollection<C2DPoint>();
        public ObservableCollection<C2DPoint> RightPoly { get; set; } = new ObservableCollection<C2DPoint>();

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

        public double? LeftPolyArea => Model.GetArea(LeftPolyName);
        public double? RightPolyArea => Model.GetArea(RightPolyName);
        public double LeftPolyPerimeter => 0;
        public double RightPolyPerimeter => 0;
        public double LeftPolyOverlappingArea => 0;
        public double RightPolyOverlappingArea => 0;
        public double LeftPolyNonOverlappingArea => 0;
        public double RightPolyNonOverlappingArea => 0;

        public SpatialMapsViewModel(ISpatialMapsModel model)
        {
            Model = model;
            Events = new EventAggregator();
            OpenLeftFileCommand = new DelegateCommand(openLeftFileSafe);
            OpenRightFileCommand = new DelegateCommand(openRightFileSafe);
            SaveLeftFileCommand = new DelegateCommand(saveLeftFileSafe);
            SaveRightFileCommand = new DelegateCommand(saveRightFileSafe);
            DrawLeftFileCommand = new DelegateCommand(drawLeftFileSafe);
            DrawRightFileCommand = new DelegateCommand(drawRightFileSafe);
        }
        private void Refresh()
        {
            var refreshEvents = Events.GetEvent<RefreshEvent>();
            refreshEvents?.Publish(true);
            var leftIsValid = Model.IsPolygonValid(LeftPolyName);
            var rightIsValid = Model.IsPolygonValid(RightPolyName);
            if (leftIsValid)
            {
                OnPropertyChanged(nameof(LeftPolyArea));
                OnPropertyChanged(nameof(LeftPolyPerimeter));
            }
            if (rightIsValid)
            {
                OnPropertyChanged(nameof(RightPolyArea));
                OnPropertyChanged(nameof(RightPolyPerimeter));
            }
            if (leftIsValid && rightIsValid)
            {
                OnPropertyChanged(nameof(LeftPolyOverlappingArea));
                OnPropertyChanged(nameof(LeftPolyNonOverlappingArea));
                OnPropertyChanged(nameof(RightPolyOverlappingArea));
                OnPropertyChanged(nameof(RightPolyNonOverlappingArea));
            }
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
            polygon.Clear();// ugly hack, other hack would be to wrap Obseravable collection into NotifyPropertyChanged
            foreach (var point in points)
            {
                polygon.Add(point);
            }
        }

        private void RedrawLeftPoly(IList<C2DPoint> points)
        {
            RedrawPoly(LeftPoly, points);
        }
        private void RedrawRightPoly(IList<C2DPoint> points)
        {
            RedrawPoly(RightPoly, points);
        }

        private void drawLeftFileSafe()
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(LeftPolyName, LeftPoly);
                var dialogResult = canvas.ShowDialog();
                if ((bool)dialogResult && Model.IsPolygonNew(canvas.viewModel.Points, canvas.viewModel.FileName))
                {
                    RedrawLeftPoly(canvas.viewModel.Points);
                    var uniqeName = Model.GetUniqueNameForPolygon(canvas.viewModel.FileName);
                    LeftPolyName = uniqeName;
                    Model.AddPolygonToDictionary(canvas.viewModel.Points, uniqeName);
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
                    RedrawRightPoly(canvas.viewModel.Points);
                    var uniqeName = Model.GetUniqueNameForPolygon(canvas.viewModel.FileName);
                    RightPolyName = uniqeName;
                    Model.AddPolygonToDictionary(canvas.viewModel.Points, uniqeName);
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
