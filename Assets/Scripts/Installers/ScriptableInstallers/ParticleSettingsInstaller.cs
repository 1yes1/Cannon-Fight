using CannonFightBase;
using System.Diagnostics.Contracts;
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

        Container.BindFactory<HitParticle, HitParticle.Factory>().ConfigurePool<HitParticle,HitParticle.Pool>(HittableParticles.HitParticle, 5);
        Container.BindFactory<TakeDamageParticle, TakeDamageParticle.Factory>().ConfigurePool<TakeDamageParticle, TakeDamageParticle.Pool>(CannonDamageHandlerParticles.TakeDamageParticle, 5);
        Container.BindFactory<FireParticle, FireParticle.Factory>().ConfigurePool<FireParticle, FireParticle.Pool>(FireControllerParticles.FireParticle, 5);
        Container.BindFactory<DamagePotionParticle, DamagePotionParticle.Factory>().ConfigurePool<DamagePotionParticle, DamagePotionParticle.Pool>(PotionParticles.DamagePotionParticle, 5);
        Container.BindFactory<HealthPotionParticle, HealthPotionParticle.Factory>().ConfigurePool<HealthPotionParticle, HealthPotionParticle.Pool>(PotionParticles.HealthPotionParticle, 5);
        Container.BindFactory<MultiballPotionParticle, MultiballPotionParticle.Factory>().ConfigurePool<MultiballPotionParticle, MultiballPotionParticle.Pool>(PotionParticles.MultiballPotionParticle, 5);
        Container.BindFactory<ChestHitParticle, ChestHitParticle.Factory>().ConfigurePool<ChestHitParticle, ChestHitParticle.Pool>(ChestParticles.HitParticle, 5);

        //Skill Particles
        Container.BindFactory<DamageSkillParticle, DamageSkillParticle.Factory>().ConfigurePool<DamageSkillParticle, DamageSkillParticle.Pool>(CannonParticleSettings.DamageSkillParticle, 2);
        Container.BindFactory<HealthSkillParticle, HealthSkillParticle.Factory>().ConfigurePool<HealthSkillParticle, HealthSkillParticle.Pool>(CannonParticleSettings.HealthSkillParticle, 2);
        Container.BindFactory<MultiballSkillParticle, MultiballSkillParticle.Factory>().ConfigurePool<MultiballSkillParticle, MultiballSkillParticle.Pool>(CannonParticleSettings.MultiBallSkillParticle, 2);
    }

}

public static class ContainerExtensions
{
    public static void ConfigurePool<Particle,Pool>(
        this FactoryToChoiceIdBinder<Particle> poolBinder,
        Object prefab,
        int initialSize) 
        where Particle : PoolableParticleBase
        where Pool : MemoryPool<IMemoryPool, Particle>
    {
        poolBinder.FromPoolableMemoryPool<Particle, Pool>(poolBinder => poolBinder
        .WithInitialSize(initialSize)
        .FromComponentInNewPrefab(prefab)
        .UnderTransformGroup(nameof(Particle)));
    }
}