using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using SpatialMaps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialMapsWpfUi
{
    public class SpatialMapsViewModel : BindableBase
    {
        public IMapsApplicationModel Model { get; set; }
        public DelegateCommand OpenLeftFileCommand { get; }
        public DelegateCommand OpenRightFileCommand { get; }

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
                Model.OpenLeftFile();
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
                Model.OpenRightFile();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is InvalidOperationException)
            {
                Model.InputOutputService.PrintToScreen(ex.Message, MessageSeverity.Error);
            }
        }
        public SpatialMapsViewModel()
        {
            IUnityContainer ioc = new UnityContainer();//temporary solution, later move to this constructor argument
            ioc.Bootstrap();
            Model = ioc.Resolve<IMapsApplicationModel>();
            OpenLeftFileCommand = new DelegateCommand(openLeftFileSafe);
            OpenRightFileCommand = new DelegateCommand(openRightFileSafe);
        }
    }
}
