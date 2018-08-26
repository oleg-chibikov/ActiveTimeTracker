using System;
using ActiveTimeTracker.ViewModel;
using ActivityTimeTracker.Contracts;
using JetBrains.Annotations;

namespace ActiveTimeTracker.View
{
    /// <summary>
    /// The time info window.
    /// </summary>
    [UsedImplicitly]
    internal sealed partial class TimeInfoWindow : ITimeInfoWindow
    {
        public TimeInfoWindow([NotNull] TimeInfoViewModel viewModel)
        {
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            InitializeComponent();
        }
    }
}