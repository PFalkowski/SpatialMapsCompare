﻿using System;
using System.IO;
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

        public string GetFileNameForRead(string initialDirectory = null, string defaultFileName = null, string filter = null)
        {
            openFileDialog1.InitialDirectory = initialDirectory;
            openFileDialog1.Filter = filter;

            openFileDialog1.FileName = defaultFileName;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else return null;
        }

        public string GetFileNameForWrite(string initialDirectory = null, string defaultFileName = null, string filter = null)
        {
            saveFileDialog1.FileName = initialDirectory;
            saveFileDialog1.Filter = filter;
            saveFileDialog1.FileName = defaultFileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            else return null;
        }

        public void PrintToScreen(string message, MessageSeverity severity)
        {
            var caption = severity == MessageSeverity.None ? MessageSeverity.Information.ToString() : severity.ToString();
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
