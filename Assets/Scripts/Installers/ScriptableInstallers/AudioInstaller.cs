using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "AudioInstaller", menuName = "CannonFight/AudioInstaller")]
    public class AudioInstaller : ScriptableObjectInstaller<AudioInstaller>
    {
        public Sounds Sounds;

        public override void InstallBindings()
        {
            Container.BindInstance(Sounds);
        }
    }

    [Serializable]
    public class Sounds
    {
        public List<GameSoundTuple> GameSounds;
        public List<MenuSoundTuple> MenuSounds;
    }

    [Serializable]
    public class GameSoundTuple: SoundTuple
    {
        public GameSound GameSound;
    }

    [Serializable]
    public class MenuSoundTuple: SoundTuple
    {
        public MenuSound MenuSound;
    }

    [Serializable]
    public abstract class SoundTuple
    {
        public AudioClip AudioClip;
        public bool IsLoop;
        public float Volume;
        public bool Spatial3D;
    }

}