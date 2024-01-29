using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AudioManager : MonoBehaviour, IInitializable
    {
        private static AudioManager _instance;

        public static AudioManager Instance => _instance;

        private Stack<AudioSource> _audioSources;

        private List<AudioSource> _usingAudioSources;

        private List<GameSoundTuple> gameSoundTuples;

        private List<MenuSoundTuple> menuSoundTuples;

        private AudioSource _loopAudioSource;


        [Inject]
        private void Construct(Sounds sounds)
        {
            this.gameSoundTuples = sounds.GameSounds;
            this.menuSoundTuples = sounds.MenuSounds;
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            DontDestroyOnLoad(gameObject);

            _audioSources = new Stack<AudioSource>();
            _usingAudioSources = new List<AudioSource>();

        }
        private void Start()
        {

        }

        public void Initialize()
        {
        }

        private void Update()
        {
            for (int i = 0; i < _usingAudioSources.Count; i++)
            {
                if(!_usingAudioSources[i].isPlaying)
                {
                    AudioSource audioSource = _usingAudioSources[i];
                    PushAudioSource(audioSource);
                }
            }
        }

        public static void PlaySound(Enum sound,Vector3 position = default(Vector3),float delay = 0)
        {
            if (_instance == null)
            {
                Debug.LogError("No AudioManager Instance");
                return;
            }

            if (sound is GameSound)
            {
                GameSoundTuple tuple = _instance.gameSoundTuples.Find(t => t.GameSound == (GameSound)sound);
                _instance.StartCoroutine(PlaySoundTimer(delay, tuple,position));
            }
            else if (sound is MenuSound)
            {
                MenuSoundTuple tuple = _instance.menuSoundTuples.Find(t => t.MenuSound == (MenuSound)sound);
                _instance.StartCoroutine(PlaySoundTimer(delay, tuple,position));
            }
            else
            {
                Debug.LogWarning("Sound Not Found!!");
            }
        }

        public static void StopSound(Enum sound)
        {

        }

        private static IEnumerator PlaySoundTimer(float delay,SoundTuple soundTuple,Vector3 position)
        {
            yield return new WaitForSeconds(delay);
            PlaySound(soundTuple, position);
        }

        private static void PlaySound(SoundTuple tuple, Vector3 position = default(Vector3))
        {
            if (tuple.IsLoop && _instance._loopAudioSource != null)
                PushAudioSource(_instance._loopAudioSource);

            AudioSource audioSource = PopAudioSource();
            audioSource.clip = tuple.AudioClip;
            audioSource.loop = tuple.IsLoop;
            audioSource.volume = tuple.Volume;

            if (tuple.IsLoop) _instance._loopAudioSource = audioSource;

            if (tuple.Spatial3D)
            {
                audioSource.spatialBlend = 1;
                audioSource.transform.position = position;
            }

            audioSource.Play();
            _instance._usingAudioSources.Add(audioSource);
        }

        private static AudioSource PopAudioSource()
        {
            if(_instance._audioSources.Count > 0)
            {
                AudioSource audioSource = _instance._audioSources.Pop();
                audioSource.gameObject.SetActive(true);

                return audioSource;
            }
            else
            {
                GameObject audioSourceObject = new GameObject("Audio Source",new Type[] {typeof(AudioSource)});
                audioSourceObject.transform.SetParent(_instance.transform);
                return audioSourceObject.GetComponent<AudioSource>();
            }
        }

        private static void PushAudioSource(AudioSource audioSource)
        {
            audioSource.Stop();
            _instance._usingAudioSources.Remove(audioSource);
            _instance._audioSources.Push(audioSource);
            audioSource.gameObject.SetActive(false);
        }


    }

    public enum GameSound
    {
        GameMusic,
        CannonFire,
        CannonHit,
        WallHit,
        CollectPoison,
        SkillStarted,
        Victory,
        Defeated,
        Countdown,
        CountdownFinished
    }

    public enum MenuSound
    {
        MenuMusic,
        ButtonClick,
        Upgrade,
    }

}
