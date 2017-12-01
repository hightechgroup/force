namespace Force.Benchmarks.Core
{
    public class CanBeActive
    {
        public bool IsActive { get; protected set; }

        public CanBeActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}