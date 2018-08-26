using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ActivityTimeTracker.Contracts;
using Common.Logging;
using JetBrains.Annotations;
using PropertyChanged;
using Scar.Common.IO;
using Scar.Common.WPF.Commands;
using Scar.Common.WPF.View;

namespace ActiveTimeTracker.ViewModel
{
    [UsedImplicitly]
    [AddINotifyPropertyChangedInterface]
    public sealed class TrayViewModel
    {
        [NotNull]
        private readonly IActivityProcessor _activityProcessor;

        [NotNull]
        private readonly ILog _logger;

        [NotNull]
        private readonly IReportSerializer _reportSerializer;

        private readonly Func<TimeInfoViewModel> _timeInfoViewModelFactory;

        [NotNull]
        private readonly IWindowFactory<ITimeInfoWindow> _timeInfoWindowFactory;

        public TrayViewModel(
            [NotNull] ILog logger,
            [NotNull] IActivityProcessor activityProcessor,
            [NotNull] IReportSerializer reportSerializer,
            Func<TimeInfoViewModel> timeInfoViewModelFactory,
            [NotNull] IWindowFactory<ITimeInfoWindow> timeInfoWindowFactory)
        {
            _activityProcessor = activityProcessor ?? throw new ArgumentNullException(nameof(activityProcessor));
            _reportSerializer = reportSerializer ?? throw new ArgumentNullException(nameof(reportSerializer));
            _timeInfoViewModelFactory = timeInfoViewModelFactory;
            _timeInfoWindowFactory = timeInfoWindowFactory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            SaveReportCommand = new CorrelationCommand(SaveReport);
            ToolTipOpenCommand = new CorrelationCommand(ToolTipOpen);
            ToolTipCloseCommand = new CorrelationCommand(ToolTipClose);
            ExitCommand = new CorrelationCommand(Exit);
            OpenSettingsFolderCommand = new CorrelationCommand(OpenSettingsFolder);
            ViewLogsCommand = new CorrelationCommand(ViewLogs);
            EditPeriodsCommand = new AsyncCorrelationCommand(EditPeriodsAsync);
        }

        [NotNull]
        public ICommand EditPeriodsCommand { get; }

        [NotNull]
        public ICommand ExitCommand { get; }

        [NotNull]
        public ICommand OpenSettingsFolderCommand { get; }

        [NotNull]
        public ICommand SaveReportCommand { get; }

        [CanBeNull]
        public TimeInfoViewModel TimeInfoViewModel { get; private set; }

        [NotNull]
        public ICommand ToolTipCloseCommand { get; }

        [NotNull]
        public ICommand ToolTipOpenCommand { get; }

        [NotNull]
        public ICommand ViewLogsCommand { get; }

        private static void OpenSettingsFolder()
        {
            Process.Start($@"{CommonPaths.SettingsPath}");
        }

        private static void ViewLogs()
        {
            Process.Start($@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Scar\ActiveTimeTracker\Logs\Full.log");
        }

        [NotNull]
        private async Task EditPeriodsAsync()
        {
            _logger.Trace("Showing edit periods window...");
            await _timeInfoWindowFactory.ShowWindowAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private void Exit()
        {
            _logger.Trace("Exiting application...");
            Application.Current.Shutdown();
        }

        private void SaveReport()
        {
            _logger.Trace("Saving report...");
            var report = _activityProcessor.GenerateReport(DateTime.Now);
            var reportPath = _reportSerializer.SerializeReport(report);
            Process.Start(reportPath);
            _logger.Info("Report is saved");
        }

        private void ToolTipClose()
        {
            TimeInfoViewModel?.Dispose();
        }

        private void ToolTipOpen()
        {
            TimeInfoViewModel = _timeInfoViewModelFactory();
        }
    }
}