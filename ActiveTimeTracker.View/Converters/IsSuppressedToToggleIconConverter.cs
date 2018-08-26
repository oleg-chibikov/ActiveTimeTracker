using System;
using System.Globalization;
using System.Windows.Data;
using JetBrains.Annotations;
using MaterialDesignThemes.Wpf;

namespace ActiveTimeTracker.View.Converters
{
    [ValueConversion(typeof(bool), typeof(PackIconKind))]
    internal sealed class IsSuppressedToToggleIconConverter : IValueConverter
    {
        [NotNull]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)(value ?? false) ? PackIconKind.ToggleSwitch : PackIconKind.ToggleSwitchOff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}