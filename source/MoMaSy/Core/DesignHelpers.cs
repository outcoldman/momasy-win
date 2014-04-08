using System;
using System.ComponentModel;
using System.Windows;

namespace outcold.MoMaSy.Core
{
    internal class DesignHelpers
    {
        private static Lazy<bool> _isInDesignMode 
            = new Lazy<bool>(() => DesignerProperties.GetIsInDesignMode(new DependencyObject()));

        public static bool DesignMode
        {
            get
            {
                return _isInDesignMode.Value;
            }
        }
    }
}
