using Microsoft.Practices.Prism.Commands;
using SpatialMaps;

namespace SpatialMapsWpfUi
{
    public interface ISpatialMapsViewModel
    {
        ISpatialMapsModel Model { get; set; }
        DelegateCommand OpenLeftFileCommand { get; }
        DelegateCommand OpenRightFileCommand { get; }
        string SelectedPath { get; set; }
    }
}