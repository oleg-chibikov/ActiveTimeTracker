using System;
using ActiveTimeTracker.Core;
using ActiveTimeTracker.View;
using ActiveTimeTracker.ViewModel;
using ActivityTimeTracker.Contracts;
using Autofac;
using Autofac.Extras.Quartz;
using JetBrains.Annotations;
using Scar.Common.Messages;
using Scar.Common.WPF.View;

namespace ActiveTimeTracker
{
    /// <summary>
    /// The app.
    /// </summary>
    internal sealed partial class App
    {
        protected override void OnStartup()
        {
            Container.Resolve<ITrayWindow>().ShowDialog();
            Container.Resolve<IJobScheduler>();
        }

        protected override void RegisterDependencies([NotNull] ContainerBuilder builder)
        {
            builder.RegisterType<AutofacScopedWindowProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GenericWindowCreator<ITimeInfoWindow>>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterGeneric(typeof(WindowFactory<>)).AsImplementedInterfaces().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(TrayViewModel).Assembly).Where(t => t.Name != "ProcessedByFody").AsSelf().InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(TrayWindow).Assembly).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(ActivityProcessor).Assembly).AsImplementedInterfaces().SingleInstance();
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(ReportingJob).Assembly));
        }

        protected override void ShowMessage(Message message)
        {
            var nestedLifeTimeScope = Container.BeginLifetimeScope();
            var viewModel = nestedLifeTimeScope.Resolve<MessageViewModel>(new TypedParameter(typeof(Message), message));
            var synchronizationContext = SynchronizationContext ?? throw new InvalidOperationException();
            synchronizationContext.Post(
                x =>
                {
                    var window = nestedLifeTimeScope.Resolve<IMessageWindow>(new TypedParameter(typeof(MessageViewModel), viewModel));
                    window.AssociateDisposable(nestedLifeTimeScope);
                    window.Restore();
                },
                null);
        }
    }
}