using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CannonFightBase
{
    public class GameEventReceiver : IGameEvents
    {
        public static event Action OnGameSceneLoadedEvent;

        public static event Action OnBeforeOurPlayerSpawnedEvent;

        public static event Action OnOurPlayerSpawnedEvent;

        public static event Action<Player> OnOurPlayerDiedEvent;

        public static event Action<Player> OnPlayerDiedEvent;

        public static event Action<Agent> OnAgentDiedEvent;

        public static event Action<Player,Player> OnKillEvent;

        public static event Action<Character, Character> OnKillAgentEvent;

        public static event Action<int> OnOurPlayerHealthChangedEvent;

        public static event Action OnMobileFireButtonClickedEvent;

        public static event Action<SkillType> OnSkillBarFilledEvent;

        public static event Action<Potion> OnPotionCollectedEvent;

        public static event Action<Skill> OnSkillEndedEvent;

        public static event Action<SkillType,float> OnBeforeSkillCountdownStartedEvent;

        public static event Action<Chest,Potion> OnChestOpenedEvent;

        public static event Action<Cannon> OnBoostStartedEvent;

        public static event Action<Cannon> OnBoostEndedEvent;

        public static event Action<int> OnLeftCannonsCountChangedEvent;

        public static event Action OnPlayerFiredEvent;

        public static event Action OnGameReadyToStartEvent;

        public static event Action OnGameStartedEvent;

        public static event Action<AgentManager> OnAgentSpawnedEvent;

        public static event Action OnWinTheGameEvent;

        public static event Action OnLoseTheGameEvent;
        
        public static event Action OnGameFinishedEvent;



        public void OnMobileFireButtonClicked() => OnMobileFireButtonClickedEvent?.Invoke();

        public void OnSkillBarFilled(SkillType skill) => OnSkillBarFilledEvent?.Invoke(skill);

        public void OnPotionCollected(Potion potion) => OnPotionCollectedEvent?.Invoke(potion);

        public void OnSkillEnded(Skill skill) => OnSkillEndedEvent?.Invoke(skill);

        public void OnChestOpened(Chest chest, Potion potion) => OnChestOpenedEvent?.Invoke(chest,potion);

        public void OnBoostStarted(Cannon cannon) => OnBoostStartedEvent?.Invoke(cannon);

        public void OnBoostEnded(Cannon cannon) => OnBoostEndedEvent?.Invoke(cannon);

        public void OnOurPlayerSpawned() => OnOurPlayerSpawnedEvent?.Invoke();

        public void OnOurPlayerHealthChanged(int health) => OnOurPlayerHealthChangedEvent?.Invoke(health);

        public void OnBeforeOurPlayerSpawned() => OnBeforeOurPlayerSpawnedEvent?.Invoke();

        public void OnGameSceneLoaded() => OnGameSceneLoadedEvent?.Invoke();

        public void OnLeftCannonsCountChanged(int leftCannonsCount) => OnLeftCannonsCountChangedEvent?.Invoke(leftCannonsCount);

        public void OnOurPlayerDied(Player player) => OnOurPlayerDiedEvent?.Invoke(player);

        public void OnPlayerDied(Player player) => OnPlayerDiedEvent?.Invoke(player);

        public void OnKill(Player killer, Player dead) => OnKillEvent?.Invoke(killer, dead);

        public void OnKill(Character killer, Character dead) => OnKillAgentEvent?.Invoke(killer, dead);

        public void OnPlayerFired() => OnPlayerFiredEvent?.Invoke();

        public void OnBeforeSkillCountdownStarted(SkillType skill, float time) => OnBeforeSkillCountdownStartedEvent?.Invoke(skill, time);

        public void OnGameReadyToStart() => OnGameReadyToStartEvent?.Invoke();

        public void OnGameStarted() => OnGameStartedEvent?.Invoke();

        public void OnAgentSpawned(AgentManager agentManager) => OnAgentSpawnedEvent?.Invoke(agentManager);

        public void OnAgentDied(Agent agent) => OnAgentDiedEvent?.Invoke(agent);

        public void OnWinTheGame() => OnWinTheGameEvent?.Invoke();

        public void OnLoseTheGame() => OnLoseTheGameEvent?.Invoke();

        public void OnGameFinished() => OnGameFinishedEvent?.Invoke();
    }
}
