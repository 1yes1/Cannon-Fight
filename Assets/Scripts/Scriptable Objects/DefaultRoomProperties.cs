using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "DefaultRoomProperties", menuName = "CannonFight/DefaultRoomProperties", order = 3)]
    public class DefaultRoomProperties : ScriptableObjectInstaller
    {
        [Header("Game Start")]
        public int MinPlayersCountToStart = 2;

        public float GameStartCountdown = 1;


        public override void InstallBindings()
        {
            Container.Bind<DefaultRoomProperties>().FromScriptableObjectResource("ScriptableObjects/DefaultRoomProperties").AsSingle();
        }
    }
}
