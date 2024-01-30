using CannonFightBase;
using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "AgentSettingsInstaller", menuName = "CannonFight/AgentSettingsInstaller", order = 6)]
public class AgentSettingsInstaller : ScriptableObjectInstaller<AgentSettingsInstaller>
{
    public GameLoadingView.BotPlayerItemSettings BotPlayerItemSettings;

    public Agent.RotateSettings AgentRotationSettings;
    public Agent.ViewSettings ViewSettings;
    public AgentTraits AgentTraits;


    public override void InstallBindings()
    {
        Container.BindInstance(BotPlayerItemSettings);
        Container.BindInstance(AgentRotationSettings);
        Container.BindInstance(ViewSettings);

        Container.BindInstance(AgentTraits.MovementSettings);
        Container.BindInstance(AgentTraits.FireSettings);
        Container.BindInstance(AgentTraits.HealthSettings);
    }


}