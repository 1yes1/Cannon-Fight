namespace CannonFightBase
{
    public enum Skills
    {
        MultiBall,
        Damage,
        Health
    }

    public interface ISkill
    {
        public void Initialize();

        public void Reset();
    }
}