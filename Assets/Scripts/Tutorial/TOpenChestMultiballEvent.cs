using CannonFightUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TOpenChestMultiballEvent : TutorialEvent
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

            Cannon.Current.CannonSkillHandler.FinishAllSkills();
            UIManager.Show<GamePanelView>(false,true);

            foreach (var item in GameObject.FindObjectsOfType<FillableBar>())
                item.ResetFilling();

            _chest.Refill(0);

        }

        private void OnPotionCollected(Potion potion)
        {
            CompleteTutorial(this);
            Debug.Log("TOpenChestMultiballEvent Completed");
        }

        private void OnChestOpened(Chest arg1, Potion potion)
        {
            potion.FillCount = 3;
        }

        private void CreatePotion(out Potion potion,Potion prefab,Chest chest)
        {
            potion = GameObject.Instantiate(prefab);
            potion.SetSkillType(SkillType.MultiBall);

            potion.GoTargetPosition(prefab.Target);

            foreach (var item in potion.GetComponentsInChildren<Transform>())
            {
                if (item != potion.transform)
                    item.gameObject.SetActive(false);
            }
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
