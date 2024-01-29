using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TOpenChestHealthEvent : TutorialEvent
    {
        private Chest _chest;

        public override void SetParameters(params object[] objects)
        {
            _chest = (Chest)objects[0];
        }

        protected override void OnEventStarted()
        {
            GameEventReceiver.OnChestOpenedEvent += OnChestOpened; ;
            GameEventReceiver.OnPotionCollectedEvent += OnPotionCollected;

            Cannon.Current.CannonSkillHandler.FinishAllSkills();
            UIManager.Show<GamePanelView>(false, true);

            foreach (var item in GameObject.FindObjectsOfType<FillableBar>())
                item.ResetFilling();
            
            _chest.Refill(2);
        }

        private void OnPotionCollected(Potion obj)
        {
            CompleteTutorial(this);
            Debug.Log("TOpenChestHealthEvent Completed");
        }

        private void OnChestOpened(Chest obj, Potion potion)
        {
            potion.FillCount = 1;
            //Debug.Log("Potion Created Again: "+ _duplicatedPotion.transform.position);
            //Debug.Log("Potion Old: "+ potion.transform.position);
        }

        protected override void OnEventUpdate()
        {
        }

        public override void Dispose()
        {
            GameEventReceiver.OnChestOpenedEvent -= OnChestOpened; ;
            GameEventReceiver.OnPotionCollectedEvent -= OnPotionCollected; ; ;
        }
    }
}
