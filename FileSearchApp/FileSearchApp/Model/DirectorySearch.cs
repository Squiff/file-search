using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileSearchApp.Model
{
    /// <summary>
    /// Provides search options for filtering search directories
    /// </summary>
    public class DirectorySearch 
    {
        public string SearchPath { get; set; }
        public bool SearchSubfolders { get; set; } = false;
        public bool SearchHidden { get; set; } = false;    

        /// <summary>
        /// Enumerates all files in the search directories
        /// </summary>
        public IEnumerable<string> EnumerateFiles()
        {
            foreach (string directory in EnumerateDirectories())
            foreach (string file in Directory.EnumerateFiles(directory))
            {
                yield return file;
            }
        }

        /// <summary>
        /// Get each of the directories to be searched
        /// </summary>
        private IEnumerable<string> EnumerateDirectories()
        {
            // return current path first
            yield return SearchPath;

            if (SearchSubfolders == false)
                yield break;

            var enumerationOptions = GetEnumerationOptions();

            var directories = Directory.EnumerateDirectories(SearchPath, "*", enumerationOptions);

            foreach (string directory in directories)
            {
                yield return directory;
            }

        }

        /// <summary>
        /// Gets EnumerationOptions based on current property values (SearchHidden, SearchSubfolders)
        /// </summary>
        private EnumerationOptions GetEnumerationOptions()
        {
            var E = new EnumerationOptions();

            // search hidden folders flag
            if (SearchHidden)
                E.AttributesToSkip = E.AttributesToSkip & ~FileAttributes.Hidden;
            else
                E.AttributesToSkip = E.AttributesToSkip | FileAttributes.Hidden;

            // search subfolders flag
            E.RecurseSubdirectories = SearchSubfolders;

            return E;
        }

    }
}
