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

        public void OnOurPlayerHealthChanged();

        public void OnMobileFireButtonClicked();

        public void OnSkillBarFilled(Skills skill);

        public void OnPotionCollected(Potion potion);

        public void OnSkillEnded(Skill skill);

        public void OnChestOpened(Chest chest);

        public void OnBoostStarted(Cannon cannon);

        public void OnBoostEnded(Cannon cannon);

        public void OnPlayerEntered(Player player);

        public void OnGameSceneLoaded();
    }

}
