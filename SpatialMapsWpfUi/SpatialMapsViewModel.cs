using GeoLib;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Prism.Events;
using SpatialMaps;

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
                SaveLeftFile();
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
                SaveRightFile();
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
                LeftPolyName = Model.ReadPolygonFromFile(fileName);
                LeftPoly.Clear();
                foreach (var point in Model.TryGetPolyByKeySafe(LeftPolyName))
                {
                    LeftPoly.Add(point);
                }
                Refresh();
            }
        }

        public void OpenRightFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForRead(null, RightPolyName, Model.FilterString);
            if (fileName != null)
            {
                RightPolyName = Model.ReadPolygonFromFile(fileName);
                RightPoly.Clear();
                foreach (var point in Model.TryGetPolyByKeySafe(RightPolyName))
                {
                    RightPoly.Add(point);
                }
                Refresh();
            }
        }

        public void SaveLeftFile()
        {
            Model.WritePolygonToFile(LeftPolyName);
        }

        public void SaveRightFile()
        {
            Model.WritePolygonToFile(RightPolyName);
        }

        private void drawLeftFileSafe()
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(LeftPolyName, LeftPoly);
                var dialogResult = canvas.ShowDialog();
                if ((bool)dialogResult && Model.IsPolygonNew(canvas.viewModel.Points, canvas.viewModel.FileName))
                {
                    LeftPoly.Clear();// ugly hack, other hack would be to wrap Obseravable collection into NotifyPropertyChanged
                    foreach (var point in canvas.viewModel.Points)
                    {
                        LeftPoly.Add(point);
                    }
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
                    RightPoly.Clear();// ugly hack, other hack would be to wrap Obseravable collection into NotifyPropertyChanged
                    foreach (var point in canvas.viewModel.Points)
                    {
                        RightPoly.Add(point);
                    }
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
