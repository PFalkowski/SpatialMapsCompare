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
        string GetFileNameForOpen(string defaultPath);
        Stream OpenFile(string path);
        void PrintMessage(string message, MessageSeverity severity);
        void PrintMessage(string message, string caption, MessageSeverity severity);
    }
}
