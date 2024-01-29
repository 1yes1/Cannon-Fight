using CannonFightUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TutorialActivator : MonoBehaviour
    {
        public static TutorialActivator _instance;

        private SpawnManager _spawnManager;

        public static TutorialActivator Instance => _instance;

        [SerializeField] private List<TutorialEventObject> _tutorialEventObjects;

        private void Awake()
        {
            _instance = this;
            _spawnManager = FindObjectOfType<SpawnManager>();
        }

        public static void SpawnCannon()
        {
            CannonManager cannonManager = Instance._spawnManager.SpawnCannonManager();

            cannonManager.Cannon.CanDoAction = true;
            GameManager.SetTutorialScene();

        #if UNITY_ANDROID
            UIManager.Show<JoystickView>();
        #endif

        }


        public static void SetActiveTutorialEvent(TutorialEventType tutorialEventType,bool active)
        {
            for (int i = 0; i < _instance._tutorialEventObjects.Count; i++)
            {
                if (_instance._tutorialEventObjects[i].Type == tutorialEventType)
                {
                    for (int j = 0; j < _instance._tutorialEventObjects[i].ActivableObjects.Count; j++)
                    {
                        _instance._tutorialEventObjects[i].ActivableObjects[j].SetActive(active);
                    }
                }
            }
        }

        public static object[] GetTutorialParameters(TutorialEventType tutorialEventType)
        {
            for (int i = 0; i < _instance._tutorialEventObjects.Count; i++)
            {
                if (_instance._tutorialEventObjects[i].Type == tutorialEventType)
                {
                    return _instance._tutorialEventObjects[i].Parameters.ToArray();
                }
            }

            return null;
        }

        public static AgentManager[] SpawnBots(int count)
        {
            return Instance._spawnManager.SpawnBots(count);
        }

        [Serializable]
        public struct TutorialEventObject
        {
            public TutorialEventType Type;
            public List<GameObject> ActivableObjects;
            public List<UnityEngine.Object> Parameters;
        }

    }
}
