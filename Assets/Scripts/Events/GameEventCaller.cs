using UnityEngine;
using System.Collections;
using System;
using Photon.Realtime;

namespace CannonFightBase
{
    public class GameEventCaller: IGameEvents
    {
        private GameEventReceiver _gameEventReceiver;

        public static GameEventCaller Instance
        {
            get
            {
                return GameManager.Instance.GameEventCaller;
            }
        }

        public GameEventCaller(GameEventReceiver gameEventReceiver)
        {
            _gameEventReceiver = gameEventReceiver;
        }

        public void OnMobileFireButtonClicked()
        {
            _gameEventReceiver.OnMobileFireButtonClicked();
        }

        public void OnSkillBarFilled(Skills skill)
        {
            _gameEventReceiver.OnSkillBarFilled(skill);
        }

        public void OnPotionCollected(Potion potion)
        {
            _gameEventReceiver.OnPotionCollected(potion);
        }

        public void OnSkillEnded(Skill skill)
        {
            _gameEventReceiver.OnSkillEnded(skill);
        }

        public void OnChestOpened(Chest chest)
        {
            _gameEventReceiver.OnChestOpened(chest);
        }

        public void OnBoostStarted(Cannon cannon)
        {
            _gameEventReceiver.OnBoostStarted(cannon);
        }

        public void OnBoostEnded(Cannon cannon)
        {
            _gameEventReceiver.OnBoostEnded(cannon);
        }

        public void OnOurPlayerSpawned()
        {
            _gameEventReceiver.OnOurPlayerSpawned();
        }

        public void OnOurPlayerHealthChanged()
        {
            _gameEventReceiver.OnOurPlayerHealthChanged();
        }

        public void BeforeOurPlayerSpawned()
        {
            _gameEventReceiver.BeforeOurPlayerSpawned();
        }

        public void OnGameSceneLoaded()
        {
            _gameEventReceiver.OnGameSceneLoaded();
        }

        public void OnPlayerEnteredRoom(Player player)
        {
            _gameEventReceiver.OnPlayerEnteredRoom(player);
        }

        public void OnPlayerLeftRoom(Player player)
        {
            _gameEventReceiver.OnPlayerLeftRoom(player);
        }

        public void OnLeftCannonsCountChanged(int leftCannonsCount)
        {
            _gameEventReceiver.OnLeftCannonsCountChanged(leftCannonsCount);
        }

        public void OnPlayerDie(Player player)
        {
            _gameEventReceiver.OnPlayerDie(player);
        }

        public void OnKill(Player killer, Player dead)
        {
            _gameEventReceiver.OnKill(killer, dead);
        }

        public void OnPlayerFired()
        {
            _gameEventReceiver.OnPlayerFired();
        }

        public void OnBeforeSkillCountdownStarted(Skills skill,float time)
        {
            _gameEventReceiver.OnBeforeSkillCountdownStarted(skill,time);
        }
    }

}
