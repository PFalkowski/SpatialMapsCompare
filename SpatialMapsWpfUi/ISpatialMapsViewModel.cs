using Microsoft.Practices.Prism.Commands;
using SpatialMaps;

namespace SpatialMapsWpfUi
{
    public interface ISpatialMapsViewModel
    {
        IMapsApplicationModel Model { get; set; }
        DelegateCommand OpenLeftFileCommand { get; }
        DelegateCommand OpenRightFileCommand { get; }
        string SelectedPath { get; set; }
    }
}