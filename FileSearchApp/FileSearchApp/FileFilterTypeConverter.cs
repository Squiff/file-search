using FileSearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace FileSearchApp
{
    class FileFilterTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (FileFilterTypeVM)value;

            switch (val)
            {
                case FileFilterTypeVM.Contains:
                    return "File Name Contains";
                case FileFilterTypeVM.NotContains:
                    return "File Name Does Not Contain";
                case FileFilterTypeVM.Regex:
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
