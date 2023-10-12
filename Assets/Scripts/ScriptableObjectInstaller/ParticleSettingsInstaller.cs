using CannonFightBase;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ParticleSettingsInstaller", menuName = "CannonFight/ParticleSettingsInstaller", order = 3)]
public class ParticleSettingsInstaller : ScriptableObjectInstaller<ParticleSettingsInstaller>
{

    public FireController.ParticleSettings FireControllerParticles;
    public CannonDamageHandler.ParticleSettings CannonDamageHandlerParticles;
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

    }
}