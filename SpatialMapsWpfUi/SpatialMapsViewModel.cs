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
        public string filterString => "XML Files|*.xml;";
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
            var fileName = Model.InputOutputService.GetFileNameForRead(null, LeftPolyName, filterString);
            if (fileName != null)
            {
                var tempPoly = Model.ReadPolygonFromFile(fileName);
                LeftPoly.Clear();
                foreach (var point in tempPoly)
                {
                    LeftPoly.Add(point);
                }
                LeftPolyName = Path.GetFileNameWithoutExtension(fileName);
                Refresh();
            }
        }

        public void OpenRightFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForRead(null, RightPolyName, filterString);
            if (fileName != null)
            {
                var tempPoly = Model.ReadPolygonFromFile(fileName);
                RightPoly.Clear();
                foreach (var point in tempPoly)
                {
                    RightPoly.Add(point);
                }
                RightPolyName = Path.GetFileNameWithoutExtension(fileName);
                Refresh();
            }
        }

        public void SaveLeftFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForWrite(null, LeftPolyName, filterString);
            if (fileName != null)
            {
                Model.WritePolygonToFile(LeftPoly, fileName);
            }
        }

        public void SaveRightFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForWrite(null, RightPolyName, filterString);
            if (fileName != null)
            {
                Model.WritePolygonToFile(RightPoly, fileName);
            }
        }

        private void drawLeftFileSafe()
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(LeftPolyName, LeftPoly);
                var dialogResult = canvas.ShowDialog();
                if ((bool)dialogResult)
                {
                    LeftPoly.Clear();// ugly hack, other hack would be to wrap Obseravable collection into NotifyPropertyChanged
                    foreach (var point in canvas.viewModel.Points)
                    {
                        LeftPoly.Add(point);
                    }
                    LeftPolyName = canvas.viewModel.FileName;
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
                if ((bool)dialogResult)
                {
                    RightPoly.Clear();// ugly hack, other hack would be to wrap Obseravable collection into NotifyPropertyChanged
                    foreach (var point in canvas.viewModel.Points)
                    {
                        RightPoly.Add(point);
                    }
                    RightPolyName = canvas.viewModel.FileName;
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
