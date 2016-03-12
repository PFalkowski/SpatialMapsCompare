using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpatialMaps
{
    public class DesktopIOService : IOService
    {
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        public Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public string GetFileNameForRead(string initialDirectory = null, string filter = null)
        {
            if (!string.IsNullOrWhiteSpace(initialDirectory))
                openFileDialog1.InitialDirectory = initialDirectory;
            if (!string.IsNullOrWhiteSpace(filter))
                openFileDialog1.Filter = filter;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else return null;
        }

        public string GetFileNameForWrite(string defaultPath = null, string filter = null)
        {
            if (!string.IsNullOrWhiteSpace(defaultPath))
                saveFileDialog1.FileName = defaultPath;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            else return null;
        }

        public void PrintToScreen(string message, MessageSeverity severity)
        {
            string caption = severity == MessageSeverity.None ? MessageSeverity.Information.ToString() : severity.ToString();
            PrintToScreen(message, caption, severity);
        }

        public void PrintToScreen(string message, string caption, MessageSeverity severity)
        {
            MessageBoxIcon icon;
            switch (severity)
            {
                case MessageSeverity.Information:
                    icon = MessageBoxIcon.Information;
                    break;
                case MessageSeverity.Warning:
                    icon = MessageBoxIcon.Warning;
                    break;
                case MessageSeverity.Error:
                    icon = MessageBoxIcon.Error;
                    break;
                case MessageSeverity.None:
                default:
                    icon = MessageBoxIcon.None;
                    break;
            }

            MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
        }
    }
}
