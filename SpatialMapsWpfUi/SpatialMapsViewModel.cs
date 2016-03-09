using GeoLib;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SpatialMaps
{
    public class SpatialMapsViewModel : BindableBase, ISpatialMapsViewModel
    {
        public ObservableCollection<C2DPoint> LeftPoly { get; set; } = new ObservableCollection<C2DPoint>();
        public ObservableCollection<C2DPoint> RightPoly { get; set; } = new ObservableCollection<C2DPoint>();
        public ISpatialMapsModel Model { get; set; }
        public DelegateCommand OpenLeftFileCommand { get; }
        public DelegateCommand OpenRightFileCommand { get; }
        public DelegateCommand SaveLeftFileCommand { get; }
        public DelegateCommand SaveRightFileCommand { get; }
        public DelegateCommand DrawLeftFileCommand { get; }
        public DelegateCommand DrawRightFileCommand { get; }

        private string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                if (_selectedPath != value)
                {
                    _selectedPath = value;
                    OnPropertyChanged(nameof(SelectedPath));
                }
            }
        }

        private void openLeftFileSafe()
        {
            try
            {
                OpenLeftFile();
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
            var fileName = Model.InputOutputService.GetFileNameForRead(Environment.CurrentDirectory);
            if (fileName != null)
            {
                var tempPoly = Model.ReadPolygonFromFile(fileName);
                LeftPoly.Clear();
                foreach (var point in tempPoly)
                {
                    LeftPoly.Add(point);
                }
            }
        }
        public void OpenRightFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForRead(Environment.CurrentDirectory);
            if (fileName != null)
            {
                var tempPoly = Model.ReadPolygonFromFile(fileName);
                RightPoly.Clear();
                foreach (var point in tempPoly)
                {
                    RightPoly.Add(point);
                }
            }

            //if (fileName != null) //assigning doesnt work with binding because collection reports it's changes, but property is not itself reporting PropertyChanged event
            //    RightPoly = new ObservableCollection<C2DPoint>(ReadPolygonFromFile(fileName));
        }

        public void SaveLeftFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForWrite(Environment.CurrentDirectory);
            if (fileName != null)
            {
                Model.WritePolygonToFile(LeftPoly, fileName);
            }
        }

        public void SaveRightFile()
        {
            var fileName = Model.InputOutputService.GetFileNameForWrite(Environment.CurrentDirectory);
            if (fileName != null)
            {
                Model.WritePolygonToFile(RightPoly, fileName);
            }
        }
        private void drawLeftFileSafe()
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(LeftPoly);
                var dialogResult = canvas.ShowDialog();
                if ((bool)dialogResult)
                {
                    LeftPoly.Clear();// ugly hack, other hack would be to wrap Obseravable collection into NotifyPropertyChanged
                    foreach (var point in canvas.viewModel.Points)
                    {
                        LeftPoly.Add(point);
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        private void drawRightFileSafe()
        {
            try
            {
                var canvas = new DrawingCanvas.MainWindow(RightPoly);
                var dialogResult = canvas.ShowDialog();
                if ((bool)dialogResult)
                {
                    RightPoly.Clear();// ugly hack, other hack would be to wrap Obseravable collection into NotifyPropertyChanged
                    foreach (var point in canvas.viewModel.Points)
                    {
                        RightPoly.Add(point);
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }

        public SpatialMapsViewModel(ISpatialMapsModel model)
        {
            Model = model;
            OpenLeftFileCommand = new DelegateCommand(openLeftFileSafe);
            OpenRightFileCommand = new DelegateCommand(openRightFileSafe);
            SaveLeftFileCommand = new DelegateCommand(saveLeftFileSafe);
            SaveRightFileCommand = new DelegateCommand(saveRightFileSafe);
            DrawLeftFileCommand = new DelegateCommand(drawLeftFileSafe);
            DrawRightFileCommand = new DelegateCommand(drawRightFileSafe);
        }
    }
}
