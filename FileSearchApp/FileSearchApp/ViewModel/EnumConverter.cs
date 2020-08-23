using FileSearchApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileSearchApp.ViewModel
{
    
    class EnumConverter
    {
        /// <summary>
        /// Converts enums from Model to the ViewModel equivalent for use by the view
        /// </summary>
        /// <typeparam name="T">Enum to convert to</typeparam>
        /// <param name="convertFrom">Enum option to convert from</param>
        /// <returns>Enum option with the same name as convertFrom</returns>
        public static T Convert<T>(Enum convertFrom) where T : struct, Enum
        {
            string fromEnumName = Enum.GetName(convertFrom.GetType(), convertFrom);

            if (fromEnumName == null)
                return default(T);

            return Enum.Parse<T>(fromEnumName);
        }
    }
}
