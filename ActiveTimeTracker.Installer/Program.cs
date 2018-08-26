using System;
using Scar.Common.Installer;

namespace ActiveTimeTracker.Installer
{
    internal static class Program
    {
        private const string BuildDir = "Build";

        private const string ProductIcon = "Icon.ico";

        private static readonly Guid UpgradeCode = new Guid("ca0d3218-0ca5-4090-b466-753e461d4529");

        private static void Main()
        {
            var fileName = $"{nameof(ActiveTimeTracker)}.exe";
            new InstallBuilder(nameof(ActiveTimeTracker), nameof(Scar), BuildDir, UpgradeCode).WithIcon(ProductIcon)
                .WithDesktopShortcut(fileName)
                .WithProgramMenuShortcut(fileName)
                .WithAutostart(fileName)
                .OpenFolderAfterInstallation()
                .LaunchAfterInstallation(fileName)
                .WithProcessTermination(fileName)
                .Build();

            // TODO: Start Menu Icon
        }
    }
}