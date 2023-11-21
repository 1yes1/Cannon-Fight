using CannonFightBase;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ParticleSettingsInstaller", menuName = "CannonFight/ParticleSettingsInstaller", order = 3)]
public class ParticleSettingsInstaller : ScriptableObjectInstaller<ParticleSettingsInstaller>
{

    public FireController.ParticleSettings FireControllerParticles;
    public CannonDamageHandler.ParticleSettings CannonDamageHandlerParticles;
    public Cannon.ParticleSettings CannonParticleSettings;
    public Hittable.ParticleSettings HittableParticles;
    public Chest.ParticleSettings ChestParticles;
    public Potion.ParticleSettings PotionParticles;

    public override void InstallBindings()
    {
        Container.BindInstance(FireControllerParticles);
        Container.BindInstance(CannonDamageHandlerParticles);
        //Container.BindInstance(HittableParticles);
        Container.BindInstance(ChestParticles);
        Container.BindInstance(PotionParticles);

        //Container.BindExecutionOrder<Hittable.ParticleSettings>(-20);
        //Container.BindFactory<CannonDamageParticle, CannonDamageParticle.Factory>()
        //.FromPoolableMemoryPool<CannonDamageParticle, CannonDamageParticle.Pool>(poolBinder => poolBinder
        //.WithInitialSize(5)
        //.FromComponentInNewPrefab(CannonDamageHandlerParticles.TakeDamageParticle)
        //.UnderTransformGroup("TakeDamageParticlePool"));

        Container.BindFactory<HitParticle, HitParticle.Factory>()
        .FromPoolableMemoryPool<HitParticle, HitParticle.Pool>(poolBinder => poolBinder
        .WithInitialSize(5)
        .FromComponentInNewPrefab(HittableParticles.HitParticle)
        .UnderTransformGroup("HitParticlePool"));

        Container.BindFactory<TakeDamageParticle, TakeDamageParticle.Factory>()
        .FromPoolableMemoryPool<TakeDamageParticle, TakeDamageParticle.Pool>(poolBinder => poolBinder
        .WithInitialSize(5)
        .FromComponentInNewPrefab(CannonDamageHandlerParticles.TakeDamageParticle)
        .UnderTransformGroup("TakeDamageParticle"));


        //Skill Particles

        Container.BindFactory<DamageSkillParticle, DamageSkillParticle.Factory>()
        .FromPoolableMemoryPool<DamageSkillParticle, DamageSkillParticle.Pool>(poolBinder => poolBinder
        .WithInitialSize(2)
        .FromComponentInNewPrefab(CannonParticleSettings.DamageSkillParticle)
        .UnderTransformGroup("DamageSkillParticle"));

        Container.BindFactory<HealthSkillParticle, HealthSkillParticle.Factory>()
        .FromPoolableMemoryPool<HealthSkillParticle, HealthSkillParticle.Pool>(poolBinder => poolBinder
        .WithInitialSize(2)
        .FromComponentInNewPrefab(CannonParticleSettings.HealthSkillParticle)
        .UnderTransformGroup("HealthSkillParticle"));

        Container.BindFactory<MultiballSkillParticle, MultiballSkillParticle.Factory>()
        .FromPoolableMemoryPool<MultiballSkillParticle, MultiballSkillParticle.Pool>(poolBinder => poolBinder
        .WithInitialSize(2)
        .FromComponentInNewPrefab(CannonParticleSettings.MultiBallSkillParticle)
        .UnderTransformGroup("MultiballSkillParticle"));

    }
}