using ActivityTimeTracker.Contracts.Data;
using JetBrains.Annotations;

namespace ActivityTimeTracker.Contracts
{
    public interface IReportSerializer
    {
        [NotNull]
        string SerializeReport([NotNull] ActivityReport report);
    }
}