using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class ChestManager : MonoBehaviour
    {
        [SerializeField] private List<Potion> _potions;

        private List<Chest> _chests;

        private Chest.Settings _chestSettings;

        private int _openedChestCount = 0;

        [Inject]
        public void Construct(Chest.Settings settings,List<Chest> chests)
        {
            _chestSettings = settings;
            _chests = chests;
        }

        private void Awake()
        {
            //CreateStack();
        }

        private void OnEnable()
        {
            GameEventReceiver.OnChestOpenedEvent += OnChestOpened;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnChestOpenedEvent -= OnChestOpened;
        }

        private void Start()
        {
            Invoke(nameof(StartFillChests), _chestSettings.StartFillTime);
        }

        private void Update()
        {
            if(Time.frameCount % 10 == 0 && _openedChestCount > 0)
                CheckRefillTimes();
        }

        private void CheckRefillTimes()
        {
            for (int i = 0; i < _chests.Count; i++)
            {
                if (_chests[i].CanRefill())
                {
                    _openedChestCount--;
                    _chests[i].Refill();
                }
            }
        }

        private void OnChestOpened(Chest obj)
        {
            _openedChestCount++;
        }

        private void FillChest()
        {
            Chest chest = _chests.Find(x => !x.IsOpened && !x.IsAlreadyOpenedOneTime);
            if (chest == null)
            {
                CancelInvoke(nameof(FillChest));
                return;
            }

            chest.Refill();
        }

        public void StartFillChests()
        {
            InvokeRepeating(nameof(FillChest), _chestSettings.StartFillFrequency, _chestSettings.StartFillFrequency);
        }

        public Potion GetPotion(Skills skill)
        {
            for (int i = 0; i < _potions.Count; i++)
            {
                if (_potions[i].Skill == skill)
                    return _potions[i];
            }
            return null;
        }

    }
}
