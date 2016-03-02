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
        public Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public string GetFileNameForOpen(string defaultPath = null)
        {
            if (!string.IsNullOrWhiteSpace(defaultPath))
                openFileDialog1.FileName = defaultPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else return null;
        }

        public void PrintMessage(string message, MessageSeverity severity)
        {
            string caption = severity == MessageSeverity.None ? MessageSeverity.Information.ToString() : severity.ToString();
            PrintMessage(message, caption, severity);
        }
        public void PrintMessage(string message, string caption, MessageSeverity severity)
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
