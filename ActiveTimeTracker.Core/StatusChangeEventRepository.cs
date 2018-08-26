using ActivityTimeTracker.Contracts;
using ActivityTimeTracker.Contracts.Data;
using JetBrains.Annotations;
using Scar.Common.DAL.LiteDB;
using Scar.Common.IO;

namespace ActiveTimeTracker.Core
{
    [UsedImplicitly]
    internal sealed class StatusChangeEventRepository : TrackedLiteDbRepository<StatusChangeEvent, int>, IStatusChangeEventRepository
    {
        public StatusChangeEventRepository()
            : base(CommonPaths.SettingsPath)
        {
        }
    }
}