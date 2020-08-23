using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace FileSearchApp.ViewModel
{
    /// <summary>
    /// provides file information for matched files
    /// </summary>
    class FileMatch
    {
        public string FilePath;
        public ICommand OpenCommand { get; }

        public FileMatch(string filePath)
        {
            FilePath = filePath;
            OpenCommand = new RelayCommand(Open);
        }

        public string Directory { get { return Path.GetDirectoryName(FilePath); } }
        public string FileName { get { return Path.GetFileName(FilePath); } }

        /// <summary>
        /// Open File with the default program
        /// </summary>
        public void Open()
        {
            var P = new Process();
            P.StartInfo = new ProcessStartInfo(FilePath) { UseShellExecute = true };
            P.Start();
        }
    }
}
