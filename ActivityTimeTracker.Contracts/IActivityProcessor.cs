using System;
using ActivityTimeTracker.Contracts.Data;
using JetBrains.Annotations;

namespace ActivityTimeTracker.Contracts
{
    public interface IActivityProcessor
    {
        [NotNull]
        ActivityReport GenerateReport(DateTime date);
    }
}