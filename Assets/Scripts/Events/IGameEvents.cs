using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Rendering;

namespace CannonFightBase
{
    public interface IGameEvents
    {
        public void BeforeOurPlayerSpawned();

        public void OnOurPlayerSpawned();

        public void OnPlayerDie(Player player);

        public void OnPlayerFired();

        public void OnKill(Player killer, Player dead);

        public void OnOurPlayerHealthChanged();

        public void OnMobileFireButtonClicked();

        public void OnSkillBarFilled(Skills skill);

        public void OnBeforeSkillCountdownStarted(Skills skill, float time);

        public void OnPotionCollected(Potion potion);

        public void OnSkillEnded(Skill skill);

        public void OnChestOpened(Chest chest);

        public void OnBoostStarted(Cannon cannon);

        public void OnBoostEnded(Cannon cannon);

        public void OnPlayerEnteredRoom(Player player);

        public void OnPlayerLeftRoom(Player player);

        public void OnLeftCannonsCountChanged(int leftCannonsCount);

        public void OnGameSceneLoaded();
    }

}
