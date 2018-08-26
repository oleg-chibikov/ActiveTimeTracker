using System;
using System.Threading.Tasks;
using ActivityTimeTracker.Contracts;
using Common.Logging;
using JetBrains.Annotations;
using Quartz;

namespace ActiveTimeTracker.Core
{
    [UsedImplicitly]
    internal sealed class ReportingJob : IJob
    {
        [NotNull]
        private readonly IActivityProcessor _activityProcessor;

        [NotNull]
        private readonly ILog _logger;

        [NotNull]
        private readonly IReportSerializer _reportSerializer;

        public ReportingJob([NotNull] IActivityProcessor activityProcessor, [NotNull] ILog logger, [NotNull] IReportSerializer reportSerializer)
        {
            _activityProcessor = activityProcessor ?? throw new ArgumentNullException(nameof(activityProcessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reportSerializer = reportSerializer ?? throw new ArgumentNullException(nameof(reportSerializer));
        }

        [NotNull]
        public Task Execute(IJobExecutionContext context)
        {
            _logger.Trace("Executing...");
            var report = _activityProcessor.GenerateReport(DateTime.Now);
            _reportSerializer.SerializeReport(report);
            _logger.Info("Executed");
            return Task.CompletedTask;
        }
    }
}