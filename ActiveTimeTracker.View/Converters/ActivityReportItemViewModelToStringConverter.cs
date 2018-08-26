using System;
using System.Globalization;
using System.Windows.Data;
using ActiveTimeTracker.Resources;
using ActiveTimeTracker.ViewModel;

namespace ActiveTimeTracker.View.Converters
{
    [ValueConversion(typeof(ActivityReportItemViewModel), typeof(string))]
    internal sealed class ActivityReportItemViewModelToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ActivityReportItemViewModel item))
            {
                return null;
            }

            var end = item.End == null ? Texts.Now : item.End.Value.ToString("HH\\:mm\\:ss");
            var start = item.Start.ToString("HH\\:mm\\:ss");

            return $"{item.PeriodType}: {start} - {end}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}