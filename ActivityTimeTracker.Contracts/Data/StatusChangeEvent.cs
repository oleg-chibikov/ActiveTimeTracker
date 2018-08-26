using Scar.Common.DAL.Model;

namespace ActivityTimeTracker.Contracts.Data
{
    public sealed class StatusChangeEvent : TrackedEntity<int>
    {
        public StatusChangeEventType StatusChangeEventType { get; set; }
        public bool IsSuppressed { get; set; }
    }
}