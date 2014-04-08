using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.DomainConverters
{
    internal class TransactionKindToBooleanConverter : IValueConverter
    {
        private Type _typeTransactionKind = typeof(TransactionKind);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Debug.Assert(value != null, "value != null");
            Debug.Assert(parameter != null, "value != null");
            
            if (parameter != null && value != null)
            {
                TransactionKind kind;
                if (Enum.TryParse<TransactionKind>(parameter.ToString(), ignoreCase: true, result: out kind))
                {
                    TransactionKind kindResult;
                    if (Enum.TryParse<TransactionKind>(value.ToString(), ignoreCase: true, result: out kindResult))
                    {
                        return kindResult == kind;
                    }
                }
                else
                {
                    Debug.Assert(false, string.Format("Couldn't parse parameter with value '{0}' to TransactionKind", parameter));
                    throw new ArgumentException("parameter");
                }
            }
            

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Debug.Assert(value != null, "value != null");
            Debug.Assert(parameter != null, "value != null");

            if (parameter != null && value != null)
            {
                TransactionKind kind;
                if (Enum.TryParse<TransactionKind>(parameter.ToString(), ignoreCase: true, result: out kind))
                {
                    bool result;
                    if (bool.TryParse(value.ToString(), out result))
                    {
                        if (result)
                            return Enum.Parse(_typeTransactionKind, parameter.ToString());
                    }
                }
                else
                {
                    Debug.Assert(false, string.Format("Couldn't parse parameter with value '{0}' to TransactionKind", parameter));
                    throw new ArgumentException("parameter");
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
