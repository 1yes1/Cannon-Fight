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
        public static event Action<Player> OnPlayerEnteredEvent;
        public static event Action BeforeOurPlayerSpawnedEvent;
        public static event Action OnOurPlayerSpawnedEvent;
        public static event Action OnOurPlayerHealthChangedEvent;
        public static event Action OnMobileFireButtonClickedEvent;
        public static event Action<Skills> OnSkillBarFilledEvent;
        public static event Action<Potion> OnPotionCollectedEvent;
        public static event Action<Skill> OnSkillEndedEvent;
        public static event Action<Chest> OnChestOpenedEvent;
        public static event Action<Cannon> OnBoostStartedEvent;
        public static event Action<Cannon> OnBoostEndedEvent;

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

        public void OnOurPlayerHealthChanged()
        {
            OnOurPlayerHealthChangedEvent?.Invoke();
        }

        public void BeforeOurPlayerSpawned()
        {
            BeforeOurPlayerSpawnedEvent?.Invoke();
        }

        public void OnPlayerEntered(Player player)
        {
            OnPlayerEnteredEvent?.Invoke(player);
        }

        public void OnGameSceneLoaded()
        {
            OnGameSceneLoadedEvent?.Invoke();
        }
    }
}
