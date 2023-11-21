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
        public void Tick();

        public void Reset();
    }
}