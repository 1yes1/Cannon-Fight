using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannonFightBase
{
    public class GameEventReceiver : IGameEvents
    {
        public static event Action OnGameSceneLoadedEvent;

        public static event Action<Player> OnPlayerEnteredRoomEvent;

        public static event Action<Player> OnPlayerLeftRoomEvent;

        public static event Action OnBeforeOurPlayerSpawnedEvent;

        public static event Action OnOurPlayerSpawnedEvent;

        public static event Action<Player> OnPlayerDieEvent;

        public static event Action<Player,Player> OnKillEvent;

        public static event Action<int> OnOurPlayerHealthChangedEvent;

        public static event Action OnMobileFireButtonClickedEvent;

        public static event Action<Skills> OnSkillBarFilledEvent;

        public static event Action<Potion> OnPotionCollectedEvent;

        public static event Action<Skill> OnSkillEndedEvent;

        public static event Action<Skills,float> OnBeforeSkillCountdownStartedEvent;

        public static event Action<Chest> OnChestOpenedEvent;

        public static event Action<Cannon> OnBoostStartedEvent;

        public static event Action<Cannon> OnBoostEndedEvent;

        public static event Action<int> OnLeftCannonsCountChangedEvent;

        public static event Action OnPlayerFiredEvent;

        public void OnMobileFireButtonClicked()
        {
            OnMobileFireButtonClickedEvent?.Invoke();
        }

        public void OnSkillBarFilled(Skills skill)
        {
            OnSkillBarFilledEvent?.Invoke(skill);
        }

        public void OnPotionCollected(Potion potion)
        {
            OnPotionCollectedEvent?.Invoke(potion);
        }

        public void OnSkillEnded(Skill skill)
        {
            OnSkillEndedEvent?.Invoke(skill);
        }

        public void OnChestOpened(Chest chest)
        {
            OnChestOpenedEvent?.Invoke(chest);
        }

        public void OnBoostStarted(Cannon cannon)
        {
            OnBoostStartedEvent?.Invoke(cannon);
        }

        public void OnBoostEnded(Cannon cannon)
        {
            OnBoostEndedEvent?.Invoke(cannon);
        }

        public void OnOurPlayerSpawned()
        {
            OnOurPlayerSpawnedEvent?.Invoke();
        }

        public void OnOurPlayerHealthChanged(int health)
        {
            OnOurPlayerHealthChangedEvent?.Invoke(health);
        }

        public void OnBeforeOurPlayerSpawned()
        {
            OnBeforeOurPlayerSpawnedEvent?.Invoke();
        }

        public void OnPlayerEnteredRoom(Player player)
        {
            OnPlayerEnteredRoomEvent?.Invoke(player);
        }

        public void OnGameSceneLoaded()
        {
            OnGameSceneLoadedEvent?.Invoke();
        }

        public void OnPlayerLeftRoom(Player player)
        {
            OnPlayerLeftRoomEvent?.Invoke(player);
        }

        public void OnLeftCannonsCountChanged(int leftCannonsCount)
        {
            OnLeftCannonsCountChangedEvent?.Invoke(leftCannonsCount);
        }

        public void OnPlayerDie(Player player)
        {
            OnPlayerDieEvent?.Invoke(player);
        }

        public void OnKill(Player killer, Player dead)
        {
            OnKillEvent?.Invoke(killer,dead);
        }

        public void OnPlayerFired()
        {
            OnPlayerFiredEvent?.Invoke();
        }

        public void OnBeforeSkillCountdownStarted(Skills skill,float time)
        {
            OnBeforeSkillCountdownStartedEvent?.Invoke(skill,time);
        }
    }
}
