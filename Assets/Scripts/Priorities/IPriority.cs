namespace Dungeonesque.Priorities
{
    public enum PriorityType
    {
        Visual,
        Audio,
        Other
    }

    public interface IPriority
    {
        /// <summary>
        ///     Weight represents the priority of the activeTarget.
        ///     In PriorityController it's used to determine the time in seconds before it's no longer a priority.
        /// </summary>
        public int UUID { get; }

        public float weight { get; }

        public PriorityType type { get; }
    }
}