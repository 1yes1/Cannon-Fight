using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CannonFightBase
{
    public class ChestManager : MonoBehaviour
    {
        private static ChestManager _instance;

        [SerializeField] private List<Chest> _chests;
        [SerializeField] private List<Potion> _potions;

        private int _openedChestCount = 0;

        public static ChestManager Instance => _instance;

        private void Awake()
        {
            if( _instance == null ) 
                _instance = this;

            _chests = FindObjectsOfType<Chest>().ToList();

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
            Invoke(nameof(StartFillChests), GameManager.DefaultChestProperties.StartFillTime);
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
            Chest chest = _chests.Find(x => x.IsOpened && !x.IsAlreadyOpened);

            if (chest == null)
            {
                CancelInvoke(nameof(FillChest));
                return;
            }

            chest.Refill();
        }

        public void StartFillChests()
        {
            InvokeRepeating(nameof(FillChest), GameManager.DefaultChestProperties.StartFillFrequency, GameManager.DefaultChestProperties.StartFillFrequency);
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
