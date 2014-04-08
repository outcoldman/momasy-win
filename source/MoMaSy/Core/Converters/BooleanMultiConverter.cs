using System;
using System.Diagnostics;
using System.Windows.Data;

namespace outcold.MoMaSy.Core.Converters
{
    internal sealed class BooleanToVisibilityMultiConverter : BooleanConverterBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Debug.Assert(values != null && values.Length > 0, "values != null && values.Length > 0");
            bool result = true;
            foreach (var value in values)
            {
                Debug.Assert(value is bool, "value is bool");
                result &= (bool)value;
            }

            return PrepareReturnValue(result, targetType, parameter);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanMultiConverter not support ConvertBack method");
        }
    }
}
