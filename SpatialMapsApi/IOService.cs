using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialMaps
{
    public interface IOService
    {
        string GetFileNameForRead(string defaultPath, string filter);
        string GetFileNameForWrite(string defaultPath, string filter);
        Stream OpenFile(string path);
        void PrintToScreen(string message, MessageSeverity severity);
        void PrintToScreen(string message, string caption, MessageSeverity severity);
    }
}
