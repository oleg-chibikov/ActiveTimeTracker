using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using JetBrains.Annotations;

namespace ActiveTimeTracker.View.Converters
{
    [ValueConversion(typeof(bool), typeof(Color))]
    internal sealed class IsSuppressedToBrushConverter : IValueConverter
    {
        [NotNull]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool?)value == true ? Brushes.CornflowerBlue : Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}