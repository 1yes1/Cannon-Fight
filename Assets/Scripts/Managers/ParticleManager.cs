using CannonFightBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using UnityEditor;
using UnityEngine;
using Zenject;
using static UnityEngine.Rendering.DebugUI;
using Object = UnityEngine.Object;

public class ParticleManager:IDisposable
{
    public static ParticleManager Instance;

    private IFactory<PoolableParticleBase>[] _particleFactories;

    public ParticleManager(HitParticle.Factory factory,
                           TakeDamageParticle.Factory factory1,
                           FireParticle.Factory factory2,
                           DamageSkillParticle.Factory factory3,
                           HealthSkillParticle.Factory factory4,
                           MultiballSkillParticle.Factory factory5)
    {
        if (Instance == null)
            Instance = this;

        _particleFactories = new IFactory<PoolableParticleBase>[] { factory, factory1, factory2,factory3,factory4,factory5 };
    }

    public static T CreateParticle<T>(Vector3 worldPosition, Transform parent = null, bool isLoop = false) where T : PoolableParticleBase
    {
        PoolableParticleBase particleSystem = null;

        for (int i = 0; i < Instance._particleFactories.Length; i++)
        {
            if (Instance._particleFactories[i] is IFactory<T>)
            {
                particleSystem = Instance._particleFactories[i].Create();
                break;
            }
        }

        if (particleSystem == null)
        {
            Debug.LogWarning("There is no injected factory in list with type " + typeof(T));
            return null;
        }

        particleSystem.transform.position = worldPosition;
        particleSystem.transform.parent = parent;

        ParticleSystem particleSystem1 = particleSystem.GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particleSystem1.main;
        main.loop = isLoop;

        return (T)particleSystem;
    }

    public static void Wait()
    {

    }

    //Yeni obje oluşturup pozisyon verip başlatıyoruz
    //public static ParticleSystem CreateAndPlay(ParticleSystem particle,Transform parent,Vector3 position,bool loop = false,bool isLocalPosition = false)
    //{
    //    ParticleSystem returnPart = null;
    //    //Eğer particle atanmışsa
    //    if(particle != null)
    //    {
    //        ParticleSystem newParticle;
    //        if(parent == null) newParticle = Instantiate(particle);
    //        else newParticle = Instantiate(particle, parent);

    //        newParticle.gameObject.SetActive(true);

    //        ParticleSystem.MainModule main = newParticle.main;
    //        main.loop = loop;
    //        //main.stopAction = ParticleSystemStopAction.Destroy;

    //        if(isLocalPosition)
    //            newParticle.gameObject.transform.localPosition = position;
    //        else
    //            newParticle.gameObject.transform.position = position;

    //        newParticle.Play();
    //        returnPart = newParticle;
    //    }
    //    return returnPart;
    //}


    ////Belirli bir süree sonra oynatıyoruz
    //public IEnumerator CreateAndPlay(ParticleSystem particle, GameObject parent, Vector3 position, bool loop,float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    //Eğer particle atanmışsa
    //    if (particle != null)
    //    {
    //        ParticleSystem newParticle = Instantiate(particle, parent.transform);
    //        newParticle.gameObject.SetActive(true);

    //        ParticleSystem.MainModule main = newParticle.main;
    //        main.loop = loop;
    //        main.stopAction = ParticleSystemStopAction.Destroy;

    //        newParticle.gameObject.transform.position = position;

    //        newParticle.Play();
    //    }
    //}



    public static ParticleSystem GetParticleTupleValue<T>(Dictionary<Type, IList> tuples,T value) where T : System.Enum
    {
        if (tuples.TryGetValue(typeof(T), out IList particleTuples))
        {
            foreach (ParticleTuple<T> tuple in particleTuples)
            {
                if (tuple.data.Equals(value))
                {
                    return tuple.particleSystem;
                }
            }
        }

        Debug.LogWarning($"No particle system found for type: {value}");
        return null;
    }

    public void Dispose()
    {
        Instance = null;
    }
}





