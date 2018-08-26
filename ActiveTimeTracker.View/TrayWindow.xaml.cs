using ActiveTimeTracker.ViewModel;
using ActivityTimeTracker.Contracts;
using JetBrains.Annotations;

namespace ActiveTimeTracker.View
{
    /// <summary>
    /// The tray window.
    /// </summary>
    [UsedImplicitly]
    internal sealed partial class TrayWindow : ITrayWindow
    {
        public TrayWindow([NotNull] TrayViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}