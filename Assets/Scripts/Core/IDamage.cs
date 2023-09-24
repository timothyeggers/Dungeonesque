namespace Dungeonesque.Core
{
    public enum DamageType
    {
        Slash,
        Blunt,
        Pierce
    }

    public interface IDamage
    {
        public DamageType Type { get; }
        public float Amount { get; }
    }
}