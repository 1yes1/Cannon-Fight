namespace CannonFightBase
{
    public enum SkillType
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