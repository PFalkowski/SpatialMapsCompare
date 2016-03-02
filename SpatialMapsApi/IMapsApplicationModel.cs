namespace SpatialMaps
{
    public interface IMapsApplicationModel
    {
        IOService InputOutputService { get; }

        void OpenLeftFile();
        void OpenRightFile();
        Polygon ReadPolygonFromFile(string fileName);
    }
}