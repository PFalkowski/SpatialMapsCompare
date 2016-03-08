using Prism.Commands;
using SpatialMaps;

namespace SpatialMaps
{
    public interface ISpatialMapsViewModel
    {
        ISpatialMapsModel Model { get; set; }
        DelegateCommand OpenLeftFileCommand { get; }
        DelegateCommand OpenRightFileCommand { get; }
        DelegateCommand SaveLeftFileCommand { get; }
        DelegateCommand SaveRightFileCommand { get; }
        DelegateCommand DrawLeftFileCommand { get; }
        DelegateCommand DrawRightFileCommand { get; }
        string SelectedPath { get; set; }
    }
}