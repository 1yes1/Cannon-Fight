using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "AnimationSettingsInstaller", menuName = "CannonFight/AnimationSettingsInstaller", order = 4)]
    public class AnimationSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public Chest.AnimatorSettings ChestAnimatorSettings;
        public DefeatedPanelView.AnimationSettings DefeatedPanelView;
        public VictoryPanelView.AnimationSettings VictoryPanelView;

        public override void InstallBindings()
        {
            Container.BindInstance(ChestAnimatorSettings);
            Container.BindInstance(DefeatedPanelView.DefeatedPanelView);

            Container.BindInstance(VictoryPanelView.VictoryPanelView);
            Container.BindInstance(VictoryPanelView.CoinGainSubView);
            Container.BindInstance(VictoryPanelView.KillCountSubView);
            Container.BindInstance(VictoryPanelView.RankSubView);
            Container.BindInstance(VictoryPanelView.ShineSubView);
        }


    }
}
