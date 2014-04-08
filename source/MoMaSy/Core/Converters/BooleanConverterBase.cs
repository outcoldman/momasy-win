using System;
using System.Windows;

namespace outcold.MoMaSy.Core.Converters
{
    internal abstract class BooleanConverterBase
    {
        protected static Type _objectType = typeof(object);
        protected static Type _booleanType = typeof(bool);
        protected static Type _visibilityType = typeof(Visibility);

        [Flags]
        internal enum Parameters
        {
            Inverse = 1,
            Hidden = 2
        }

        protected object PrepareReturnValue(bool result, Type targetType, object parameter)
        {
            var parameters = ParseParameters(parameter);

            if (targetType == _booleanType || _objectType == targetType)
                return BooleanToBoolean(result, parameters);
            else if (targetType == _visibilityType)
                return BooleanToVisibility(result, parameters);

            throw new NotSupportedException(string.Format("Type '{0}' is not supported by method Convert of type '{1}'.", targetType, GetType()));
        }

        protected object PrepareReturnValue(Visibility result, Type targetType, object parameter)
        {
            if (targetType == _booleanType)
                return VisibilityToBoolean(result, ParseParameters(parameter));

            throw new NotSupportedException(string.Format("Type '{0}' is not supported by method ConvertBack of type '{1}'.", targetType, GetType()));
        }

        private Parameters ParseParameters(object parameter)
        {
            if (parameter == null)
                return 0;
            return (Parameters)Enum.Parse(typeof(Parameters), parameter.ToString()); ;
        }

        protected bool BooleanToBoolean(bool value, Parameters parameters)
        {
            if (((parameters & Parameters.Inverse) == Parameters.Inverse))
                return !value;
            return value;
        }

        protected Visibility BooleanToVisibility(bool value, Parameters parameters)
        {
            return BooleanToBoolean(value, parameters)
                ? Visibility.Visible : ((parameters & Parameters.Hidden) == Parameters.Hidden)
                                            ? Visibility.Hidden : Visibility.Collapsed;
        }

        protected bool VisibilityToBoolean(Visibility value, Parameters parameters)
        {
            return (value == Visibility.Visible ^ ((parameters & Parameters.Inverse) == Parameters.Inverse));
        }
    }
}
