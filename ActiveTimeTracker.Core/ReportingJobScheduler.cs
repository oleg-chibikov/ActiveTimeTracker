using System;
using ActivityTimeTracker.Contracts;
using JetBrains.Annotations;
using Quartz;

namespace ActiveTimeTracker.Core
{
    [UsedImplicitly]
    internal sealed class ReportingJobScheduler : IJobScheduler, IDisposable
    {
        [NotNull]
        private readonly IScheduler _scheduler;

        public ReportingJobScheduler([NotNull] IScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            var job = JobBuilder.Create<ReportingJob>().WithIdentity("myJob", "group1").Build();
            var trigger = TriggerBuilder.Create().WithIdentity("myTrigger", "group1").StartNow().WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever()).Build();

            _scheduler.ScheduleJob(job, trigger);
            _scheduler.Start();
        }

        public void Dispose()
        {
            _scheduler.Shutdown();
        }
    }
}