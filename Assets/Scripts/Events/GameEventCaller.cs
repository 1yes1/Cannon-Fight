using UnityEngine;
using System.Collections;
using System;
using Photon.Realtime;

namespace CannonFightBase
{
    public class GameEventCaller: IGameEvents
    {
        private GameEventReceiver _gameEventReceiver;

        public static GameEventCaller Instance => GameManager.Instance.GameEventCaller;

        public GameEventCaller(GameEventReceiver gameEventReceiver) => _gameEventReceiver = gameEventReceiver;

        public void OnMobileFireButtonClicked() => _gameEventReceiver.OnMobileFireButtonClicked();

        public void OnSkillBarFilled(SkillType skill) => _gameEventReceiver.OnSkillBarFilled(skill);

        public void OnPotionCollected(Potion potion) => _gameEventReceiver.OnPotionCollected(potion);

        public void OnSkillEnded(Skill skill) => _gameEventReceiver.OnSkillEnded(skill);

        public void OnChestOpened(Chest chest) => _gameEventReceiver.OnChestOpened(chest);

        public void OnBoostStarted(Cannon cannon) => _gameEventReceiver.OnBoostStarted(cannon);

        public void OnBoostEnded(Cannon cannon) => _gameEventReceiver.OnBoostEnded(cannon);

        public void OnOurPlayerSpawned() => _gameEventReceiver.OnOurPlayerSpawned();

        public void OnOurPlayerHealthChanged(int health) => _gameEventReceiver.OnOurPlayerHealthChanged(health);

        public void OnBeforeOurPlayerSpawned() => _gameEventReceiver.OnBeforeOurPlayerSpawned();

        public void OnGameSceneLoaded() => _gameEventReceiver.OnGameSceneLoaded();

        public void OnLeftCannonsCountChanged(int leftCannonsCount) => _gameEventReceiver.OnLeftCannonsCountChanged(leftCannonsCount);

        public void OnOurPlayerDied(Player player) => _gameEventReceiver.OnOurPlayerDied(player);

        public void OnPlayerDied(Player player) => _gameEventReceiver.OnPlayerDied(player);

        public void OnKill(Player killer, Player dead) => _gameEventReceiver.OnKill(killer, dead);

        public void OnKill(Character killer, Character dead) => _gameEventReceiver.OnKill(killer, dead);

        public void OnPlayerFired() => _gameEventReceiver.OnPlayerFired();

        public void OnBeforeSkillCountdownStarted(SkillType skill, float time) => _gameEventReceiver.OnBeforeSkillCountdownStarted(skill, time);

        public void OnGameReadyToStart() => _gameEventReceiver.OnGameReadyToStart();

        public void OnGameStarted() => _gameEventReceiver.OnGameStarted();

        public void OnAgentSpawned(AgentManager agentManager) => _gameEventReceiver.OnAgentSpawned(agentManager);

        public void OnAgentDied(Agent agent) => _gameEventReceiver.OnAgentDied(agent);

    }

}