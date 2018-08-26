using System;
using System.Collections.Generic;
using System.Linq;
using ActivityTimeTracker.Contracts;
using ActivityTimeTracker.Contracts.Data;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace ActiveTimeTracker.Core
{
    [UsedImplicitly]
    internal sealed class ActivityProcessor : IActivityProcessor
    {
        [NotNull]
        private readonly IStatusChangeEventRepository _statusChangeEventRepository;

        public ActivityProcessor([NotNull] IStatusChangeEventRepository statusChangeEventRepository)
        {
            _statusChangeEventRepository = statusChangeEventRepository ?? throw new ArgumentNullException(nameof(statusChangeEventRepository));
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        public ActivityReport GenerateReport(DateTime date)
        {
            var start = date.Date;
            var end = date.Date.AddDays(1);
            var todaysActivity = _statusChangeEventRepository.GetCreatedBetween(start, end).OrderBy(x => x.CreatedDate).Cast<StatusChangeEvent>().ToArray();
            var lastStartWorkingEvent = new StatusChangeEvent
            {
                CreatedDate = start.AddHours(8)
            }; // assume that if first event of the Day is Lock/Logoff then the day was started at 8AM

            StatusChangeEvent lastStartLeisureEvent = null;
            var addLast = true;
            var reportItems = new List<ActivityReportItem>();
            foreach (var activityItem in todaysActivity)
            {
                if (activityItem.StatusChangeEventType == StatusChangeEventType.Logon || activityItem.StatusChangeEventType == StatusChangeEventType.Unlock)
                {
                    addLast = true;
                    lastStartWorkingEvent = activityItem;
                    if (lastStartLeisureEvent == null)
                    {
                        continue;
                    }

                    var reportItem = new ActivityReportItem(PeriodType.Leisure, lastStartLeisureEvent, activityItem);
                    reportItems.Add(reportItem);
                }
                else
                {
                    addLast = false;
                    var reportItem = new ActivityReportItem(PeriodType.Working, lastStartWorkingEvent, activityItem);
                    reportItems.Add(reportItem);
                    lastStartLeisureEvent = activityItem;
                }
            }

            if (addLast)
            {
                reportItems.Add(new ActivityReportItem(PeriodType.Working, lastStartWorkingEvent, null));
            }

            return new ActivityReport(reportItems, date);
        }

        private void SystemEvents_SessionSwitch(object sender, [NotNull] SessionSwitchEventArgs e)
        {
            StatusChangeEventType eventType;
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLogoff:
                    eventType = StatusChangeEventType.Logoff;
                    break;
                case SessionSwitchReason.SessionLock:
                    eventType = StatusChangeEventType.Lock;
                    break;
                case SessionSwitchReason.SessionLogon:
                    eventType = StatusChangeEventType.Logon;
                    break;
                case SessionSwitchReason.SessionUnlock:
                    eventType = StatusChangeEventType.Unlock;
                    break;
                default:
                    return;
            }

            var entity = new StatusChangeEvent
            {
                StatusChangeEventType = eventType
            };

            _statusChangeEventRepository.Insert(entity);
        }
    }
}