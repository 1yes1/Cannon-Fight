using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class TestClass : MonoBehaviour
    {
        Launcher.Settings _settings;

        [Inject]
        public void Construct(Launcher.Settings settings)
        {
            _settings = settings;

            Debug.Log("Settingss: " + settings.MinPlayersCountToStart);
        }

        public void Start()
        {
            Debug.Log("Initialize: " + _settings.MinPlayersCountToStart);

        }
    }
}
