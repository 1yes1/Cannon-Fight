using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CannonFightBase
{
    public class LoadSceneManager : IInitializable,IDisposable,ITickable
    {
        private static int _targetSceneIndex;
        
        private static bool _isPhotonLoad = false;

        private static float _loadTime = 0;

        private static bool _canLoad = false;

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        public static void LoadScene(GameScene scene,float delay = 0)
        {
            _targetSceneIndex = (int)scene;
            _isPhotonLoad = false;
            _loadTime = delay;
            _canLoad = true;
        }

        public static void PhotonLoadScene(GameScene scene, float delay = 0)
        {
            _targetSceneIndex = (int)scene;
            _isPhotonLoad = true;
            _loadTime = delay;
            _canLoad =true;
        }

        public void Tick()
        {
            if(_loadTime >= 0 && _canLoad)
            {
                _loadTime -= Time.deltaTime;
                if(_loadTime <= 0)
                {
                    _canLoad = false;

                    if (_isPhotonLoad) 
                        PhotonNetwork.LoadLevel(_targetSceneIndex);
                    else
                        SceneManager.LoadScene(_targetSceneIndex);
                }
            }
        }

    }

    public enum GameScene
    {
        Menu,
        Tutorial,
        Game
    }

}
