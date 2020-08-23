using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileSearchApp.Model
{
    /// <summary>
    /// provides search options for filtering files
    /// </summary>
    public class FileFilter
    {

        public FileFilterType SearchType { get; set; } = FileFilterType.Contains;
        public string Value { get; set; }

        /// <summary>
        /// Check whether filter provides a match for the input file
        /// </summary>
        public bool Evaluate(string filePath)
        {
            if (string.IsNullOrEmpty(Value))
                return true;
                

            string fileName = Path.GetFileName(filePath);

            switch (SearchType)
            {
                case FileFilterType.Contains:
                    return EvaluateContains(fileName);
                case FileFilterType.NotContains:
                    return !(EvaluateContains(fileName));
                case FileFilterType.Regex:
                    return EvaluateRegex(fileName);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Evaluate whether the value is contained in the filename. * and ? can be used as wildcards.
        /// </summary>
        private bool EvaluateContains(string fileName)
        {
            // escape regex chars and allow * ? wildcard searches
            string pattern = Regex.Escape(Value)
                                    .Replace(@"\*", ".*")
                                    .Replace(@"\?", ".");

            var R = new Regex(pattern, RegexOptions.IgnoreCase);

            var M = R.Match(fileName);

            return M.Success;
        }

        /// <summary>
        /// Evaluate the provided file name against the filters regex
        /// </summary>
        private bool EvaluateRegex(string fileName)
        {
            var R = new Regex(Value, RegexOptions.IgnoreCase);

            var M = R.Match(fileName);

            return M.Success;
        }
    }
}
