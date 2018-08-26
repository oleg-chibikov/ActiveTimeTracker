using System;
using ActiveTimeTracker.ViewModel;
using ActivityTimeTracker.Contracts;
using JetBrains.Annotations;

namespace ActiveTimeTracker.View
{
    /// <summary>
    /// The message window.
    /// </summary>
    [UsedImplicitly]
    internal sealed partial class MessageWindow : IMessageWindow
    {
        public MessageWindow([NotNull] MessageViewModel viewModel)
        {
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            InitializeComponent();
        }
    }
}