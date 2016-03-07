using Prism.Commands;
using SpatialMaps;

namespace SpatialMaps
{
    public interface ISpatialMapsViewModel
    {
        ISpatialMapsModel Model { get; set; }
        DelegateCommand OpenLeftFileCommand { get; }
        DelegateCommand OpenRightFileCommand { get; }
        string SelectedPath { get; set; }
    }
}