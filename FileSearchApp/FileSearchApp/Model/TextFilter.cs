using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FileSearchApp.Model
{
    /// <summary>
    /// Class to provide text search values and options
    /// </summary>
    public class TextFilter
    {
        public TextFilterType SearchType { get; set; } = TextFilterType.Contains;
        public string Value { get; set; } = string.Empty;
        public bool MatchCase { get; set; } = false;
        public bool MatchWord { get; set; } = false;

        /// <summary>
        /// Gets RegexOptions based on current property values (Match Case)
        /// </summary>
        private RegexOptions FilterRegexOptions
        {
            get
            {
                RegexOptions options = RegexOptions.None;

                if (MatchCase == false)
                    options = options | RegexOptions.IgnoreCase;

                return options;
            }
        }

        /// <summary>
        /// Test whether the filter provides a match for the provided text
        /// </summary>
        public bool Evaluate(string input)
        {
            // blank value means do not filter
            if (string.IsNullOrEmpty(Value))
                return true;

            switch (SearchType)
            {
                case TextFilterType.Contains:
                    return EvaluateContains(input);
                case TextFilterType.NotContains:
                    return !(EvaluateContains(input));
                case TextFilterType.Regex:
                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Evaluate whether the value is contained in the filename. 
        /// </summary>
        private bool EvaluateContains(string input)
        {
            string pattern = Regex.Escape(Value);

            if (MatchWord)
                pattern = @"\b" + pattern + @"\b";

            var R = new Regex(pattern, FilterRegexOptions);

            var M = R.Match(input);

            return M.Success;
        }

        /// <summary>
        /// Evaluate the input string against the filters regex
        /// </summary>
        private bool EvaluateRegex(string input)
        {
            var R = new Regex(Value, FilterRegexOptions);

            var M = R.Match(input);

            return M.Success;
        }

        

    }
}
