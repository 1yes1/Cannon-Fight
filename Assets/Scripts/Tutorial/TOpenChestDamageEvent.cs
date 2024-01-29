using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TOpenChestDamageEvent : TutorialEvent
    {
        private Chest _chest;

        public override void SetParameters(params object[] objects)
        {
            _chest = (Chest)objects[0];
        }

        protected override void OnEventStarted()
        {
            GameEventReceiver.OnChestOpenedEvent += OnChestOpened; ;
            GameEventReceiver.OnPotionCollectedEvent += OnPotionCollected; ; ;

            UIManager.Show<GamePanelView>(false,true);
            GameManager.Instance.LeftCannonsCount = 0;
            _chest.Refill(1);
        }

        private void OnPotionCollected(Potion obj)
        {
            CompleteTutorial(this);
            Debug.Log("TOpenChestDamageEvent Completed");
        }

        private void OnChestOpened(Chest obj, Potion potion)
        {
            potion.FillCount = 2;
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
