using System;
using System.Collections.Generic;
using System.Linq;

namespace ActivityTimeTracker.Contracts.Data
{
    public sealed class ActivityReport
    {
        private readonly TimeSpan _totalLeisureTime;

        private readonly TimeSpan _totalWorkingTime;

        public ActivityReport(ICollection<ActivityReportItem> items, DateTime reportDate)
        {
            Items = items;
            ReportDate = reportDate;
            _totalWorkingTime = GetTime(PeriodType.Working);
            _totalLeisureTime = GetTime(PeriodType.Leisure);
        }

        public ICollection<ActivityReportItem> Items { get; }

        public DateTime ReportDate { get; }

        public TimeSpan TotalLeisureTime => GetTimeToCurrent(_totalLeisureTime, PeriodType.Leisure);

        public TimeSpan TotalWorkingTime => GetTimeToCurrent(_totalWorkingTime, PeriodType.Working);

        private TimeSpan GetTime(PeriodType periodType)
        {
            return Items.Where(x => x.PeriodType == periodType).Aggregate(TimeSpan.Zero, (x, y) => y.Period != null ? x + y.Period.Value : x);
        }

        private TimeSpan GetTimeToCurrent(TimeSpan time, PeriodType periodType)
        {
            var last = Items.LastOrDefault();
            if (last != null && last.End == null)
            {
                if (last.PeriodType == periodType)
                {
                    return time + (DateTime.Now - last.Start);
                }
            }

            return time;
        }
    }
}