using ActivityTimeTracker.Contracts.Data;
using Scar.Common.DAL;

namespace ActivityTimeTracker.Contracts
{
    public interface IStatusChangeEventRepository : IRepository<StatusChangeEvent, int>, ITrackedRepository
    {
    }
}