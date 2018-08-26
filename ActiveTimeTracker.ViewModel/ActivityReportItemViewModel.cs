using System;
using ActivityTimeTracker.Contracts.Data;
using JetBrains.Annotations;
using PropertyChanged;

namespace ActiveTimeTracker.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public sealed class ActivityReportItemViewModel
    {
        public ActivityReportItemViewModel([NotNull] ActivityReportItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            PeriodType = item.PeriodType;
            Start = item.Start;
            End = item.End;
            StartEventId = item.StartEventId;
            Period = item.Period;
            IsSuppressed = item.IsSuppressed;
        }

        public DateTime? End { get; }

        public bool IsSuppressed { get; set; }

        public TimeSpan? Period { get; }

        public PeriodType PeriodType { get; }

        public DateTime Start { get; }

        public int StartEventId { get; }
    }
}