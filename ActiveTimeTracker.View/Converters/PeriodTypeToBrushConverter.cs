using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ActivityTimeTracker.Contracts.Data;
using JetBrains.Annotations;

namespace ActiveTimeTracker.View.Converters
{
    [ValueConversion(typeof(PeriodType), typeof(Color))]
    internal sealed class PeriodTypeToBrushConverter : IValueConverter
    {
        // TODO: Gray color for suppressed. PeriodInfoViewModel class!
        [NotNull]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Brushes.Black;
            }

            switch ((PeriodType)value)
            {
                case PeriodType.Working:
                    return Brushes.ForestGreen;
                case PeriodType.Leisure:
                    return Brushes.OrangeRed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}