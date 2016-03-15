using GeoLib;
using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;

namespace SpatialMaps
{
    public interface ISpatialMapsViewModel
    {
        ISpatialMapsModel Model { get; set; }
        IEventAggregator Events { get; }
        ObservableCollection<C2DPoint> LeftPoly { get; set; }
        ObservableCollection<C2DPoint> RightPoly { get; set; }
        DelegateCommand OpenLeftFileCommand { get; }
        DelegateCommand OpenRightFileCommand { get; }
        DelegateCommand SaveLeftFileCommand { get; }
        DelegateCommand SaveRightFileCommand { get; }
        DelegateCommand DrawLeftFileCommand { get; }
        DelegateCommand DrawRightFileCommand { get; }
    }
}