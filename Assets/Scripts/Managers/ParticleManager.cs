using CannonFightBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using UnityEditor;
using UnityEngine;
using Zenject;
using static UnityEngine.Rendering.DebugUI;
using Object = UnityEngine.Object;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager _instance;

    public static ParticleManager Instance => _instance;

    private void Awake()
    {
        if(_instance == null)
            _instance = this;

    }


    public static T CreateWithFactory<T>(PlaceholderFactory<T> placeholderFactory, Vector3 worldPosition, Transform parent, bool isLoop)
    {
        T particleSystem = placeholderFactory.Create();

        MonoBehaviour particle = (particleSystem as MonoBehaviour);

        particle.transform.position = worldPosition;
        particle.transform.parent = parent;

        ParticleSystem particleSystem1 = particle.GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particleSystem1.main;
        main.loop = isLoop;

        return particleSystem;
    }


    //Zaten sahnede olanı oynatıyoruz
    public static void Play(ParticleSystem particle, bool loop)
    {
        if (particle != null)
        {
            particle.gameObject.SetActive(true);
            ParticleSystem.MainModule main = particle.main;

            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;
            particle.Play();
        }

    }


    public static void Play(ParticleSystem particle, bool loop,Vector3 pos)
    {
        if (particle != null)
        {
            particle.gameObject.SetActive(true);
            particle.gameObject.transform.position = pos;
            ParticleSystem.MainModule main = particle.main;

            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;
            particle.Play();
        }

    }


    public static void Stop(ParticleSystem particle,bool destroy)
    {
        ParticleSystem.MainModule main = particle.main;

        if(destroy)
            main.stopAction = ParticleSystemStopAction.Destroy;
        else
            main.stopAction = ParticleSystemStopAction.None;

        particle.Stop();
    }

    //Yeni obje oluşturup pozisyon verip başlatıyoruz
    public static ParticleSystem CreateAndPlay(ParticleSystem particle,Transform parent,Vector3 position,bool loop = false,bool isLocalPosition = false)
    {
        ParticleSystem returnPart = null;
        //Eğer particle atanmışsa
        if(particle != null)
        {
            ParticleSystem newParticle;
            if(parent == null) newParticle = Instantiate(particle);
            else newParticle = Instantiate(particle, parent);

            newParticle.gameObject.SetActive(true);

            ParticleSystem.MainModule main = newParticle.main;
            main.loop = loop;
            //main.stopAction = ParticleSystemStopAction.Destroy;

            if(isLocalPosition)
                newParticle.gameObject.transform.localPosition = position;
            else
                newParticle.gameObject.transform.position = position;

            newParticle.Play();
            returnPart = newParticle;
        }
        return returnPart;
    }


    //Belirli bir süree sonra oynatıyoruz
    public IEnumerator CreateAndPlay(ParticleSystem particle, GameObject parent, Vector3 position, bool loop,float time)
    {
        yield return new WaitForSeconds(time);
        //Eğer particle atanmışsa
        if (particle != null)
        {
            ParticleSystem newParticle = Instantiate(particle, parent.transform);
            newParticle.gameObject.SetActive(true);

            ParticleSystem.MainModule main = newParticle.main;
            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;

            newParticle.gameObject.transform.position = position;

            newParticle.Play();
        }
    }



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
    



}





