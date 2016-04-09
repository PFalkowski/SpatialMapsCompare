using System.IO;

namespace SpatialMaps
{
    public interface IOService
    {
        string GetFileNameForRead(string defaultPath, string defaultFileName, string filter);
        string GetFileNameForWrite(string defaultPath, string defaultFileName, string filter);
        Stream OpenFile(string path);
        void PrintToScreen(string message, MessageSeverity severity);
        void PrintToScreen(string message, string caption, MessageSeverity severity);
    }
}
