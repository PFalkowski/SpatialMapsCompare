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
        DelegateCommand RefreshCommand { get; }
        DelegateCommand SnapToOriginLeftCommand { get; }
        DelegateCommand SnapToOriginRightCommand { get; }
        double? LeftPolyArea { get; }
        double? RightPolyArea { get; }
        double? AreaDifference { get; }
        double? LeftPolyPerimeter { get; }
        double? RightPolyPerimeter { get; }
        double? PerimeterDifference { get; }
        double? LeftPolyOverlappingArea { get; }
        double? RightPolyOverlappingArea { get; }
        double? LeftPolyNonOverlappingArea { get; }
        double? RightPolyNonOverlappingArea { get; }
        double? OverlappingAreasSum { get; }
        double? NonOverlappingAreasSum { get; }
        double? ResemblenceIndex { get; }
        void Refresh();
    }
}