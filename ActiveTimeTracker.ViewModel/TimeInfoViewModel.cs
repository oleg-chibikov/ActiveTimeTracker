using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using ActivityTimeTracker.Contracts;
using ActivityTimeTracker.Contracts.Data;
using Common.Logging;
using JetBrains.Annotations;
using PropertyChanged;
using Scar.Common.WPF.Commands;

namespace ActiveTimeTracker.ViewModel
{
    [UsedImplicitly]
    [AddINotifyPropertyChangedInterface]
    public sealed class TimeInfoViewModel : IDisposable
    {
        [NotNull]
        private readonly ActivityReport _report;

        [NotNull]
        private readonly IStatusChangeEventRepository _statusChangeEventRepository;

        [NotNull]
        private readonly DispatcherTimer _timer;

        public TimeInfoViewModel(
            [NotNull] ILog logger,
            [NotNull] IActivityProcessor activityProcessor,
            [NotNull] IReportSerializer reportSerializer,
            [NotNull] IStatusChangeEventRepository statusChangeEventRepository)
        {
            _statusChangeEventRepository = statusChangeEventRepository ?? throw new ArgumentNullException(nameof(statusChangeEventRepository));
            activityProcessor = activityProcessor ?? throw new ArgumentNullException(nameof(activityProcessor));

            ToggleCommand = new CorrelationCommand<ActivityReportItemViewModel>(Toggle);
            _report = activityProcessor.GenerateReport(DateTime.Now);
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Start();
            _report = activityProcessor.GenerateReport(DateTime.Now);
            ActivityReportItems.Clear();
            foreach (var viewModelItem in _report.Items.Select(item => new ActivityReportItemViewModel(item)))
            {
                ActivityReportItems.Add(viewModelItem);
            }

            SetCurrentTimeInfo();
            _timer.Tick += Timer_Tick;
        }

        [NotNull]
        public ObservableCollection<ActivityReportItemViewModel> ActivityReportItems { get; } = new ObservableCollection<ActivityReportItemViewModel>();

        [NotNull]
        public ICommand ToggleCommand { get; }

        public TimeSpan TotalLeisureTimeForToday { get; private set; }

        public TimeSpan TotalWorkingTimeForToday { get; private set; }

        public void Dispose()
        {
            _timer.Tick -= Timer_Tick;
            _timer.Stop();
        }

        private void SetCurrentTimeInfo()
        {
            TotalWorkingTimeForToday = _report.TotalWorkingTime;
            TotalLeisureTimeForToday = _report.TotalLeisureTime;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SetCurrentTimeInfo();
        }

        private void Toggle([NotNull] ActivityReportItemViewModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            // Should not be able to suppress 08:00:00 start event - with no Id (it's a rare ocasion - so just hide the button)
            var startEvent = _statusChangeEventRepository.GetById(item.StartEventId);
            startEvent.IsSuppressed = !startEvent.IsSuppressed;
            _statusChangeEventRepository.Update(startEvent);
            item.IsSuppressed = startEvent.IsSuppressed;
        }
    }
}