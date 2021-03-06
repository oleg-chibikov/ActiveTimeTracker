using Scar.Common.DAL.Model;

namespace ActivityTimeTracker.Contracts.Data
{
    public sealed class StatusChangeEvent : TrackedEntity<int>
    {
        public bool IsSuppressed { get; set; }

        public StatusChangeEventType StatusChangeEventType { get; set; }
    }
}