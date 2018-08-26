using System;
using System.Globalization;
using System.Windows.Data;
using ActivityTimeTracker.Contracts;

namespace ActiveTimeTracker.View.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    internal sealed class TimeSpanToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TimeSpan?)value)?.ToPrettyFormat();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}