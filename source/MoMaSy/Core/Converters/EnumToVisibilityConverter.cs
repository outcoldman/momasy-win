using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace outcold.MoMaSy.Core.Converters
{
    internal sealed class EnumToVisibilityConverter : BooleanConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(targetType == typeof(Visibility));

            if (parameter == null)
                throw new ArgumentNullException("parameter");

            if (value != null)
            {
                Type valueType = value.GetType();
                if (!valueType.IsEnum)
                    throw new ArgumentException("Value should be Enum.", "value");

                if (!Enum.IsDefined(valueType, parameter))
                    throw new ArgumentException(string.Format("Couldn't convert parameter '{0}' to enum '{1}'.", parameter, valueType), "parameter");

                var parameterValue = Enum.Parse(valueType, parameter.ToString());
                return PrepareReturnValue(Enum.Equals(parameterValue, value), targetType, ConverterParameters);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("EnumToVisibilityConverter not support ConvertBack method");
        }

        public Parameters ConverterParameters
        {
            get;
            set;
        }
    }
}
