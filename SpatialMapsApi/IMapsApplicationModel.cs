namespace SpatialMaps
{
    public interface IMapsApplicationModel
    {
        IOService InputOutputService { get; }
        Polygon LeftPoly { get; }
        Polygon RightPoly { get; }

        void OpenLeftFile();
        void OpenRightFile();
        Polygon ReadPolygonFromFile(string fileName);
    }
}