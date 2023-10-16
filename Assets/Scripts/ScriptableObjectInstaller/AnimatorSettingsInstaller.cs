using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "AnimatorSettingsInstaller", menuName = "CannonFight/AnimatorSettingsInstaller", order = 4)]
    public class AnimatorSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public Chest.AnimatorSettings ChestAnimatorSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(ChestAnimatorSettings);
        }


    }
}
