using FileSearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace FileSearchApp
{
    class TextFilterTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (TextFilterTypeVM)value;

            switch (val)
            {
                case TextFilterTypeVM.Contains:
                    return "Contains";
                case TextFilterTypeVM.NotContains:
                    return "Does Not Contain";
                case TextFilterTypeVM.Regex:
                    return "Regex";
                default:
                    return "Unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
