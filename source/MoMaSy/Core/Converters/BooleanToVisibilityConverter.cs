using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace outcold.MoMaSy.Core.Converters
{
    /// <summary>
    /// Convert Boolean type to Visibility. True to Visible and false to Collapsed. 
    /// You can put "OtherWay" value to converter parameter to change this relation.
    /// </summary>
    internal sealed class BooleanToVisibilityConverter : BooleanConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool bValue;
                if (bool.TryParse(value.ToString(), out bValue))
                {
                    return PrepareReturnValue(bValue, targetType, parameter);
                }
            }

            return PrepareReturnValue(false, targetType, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (Enum.IsDefined(_visibilityType, value))
                    return PrepareReturnValue(((Visibility)Enum.Parse(_visibilityType, value.ToString(), false)), targetType, parameter);
            }

            return PrepareReturnValue(Visibility.Collapsed, targetType, parameter); ;
        }
    }
}
