using CannonFightUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class TestManager : MonoBehaviour
    {
        [SerializeField] private bool _spawnForTest;
        [SerializeField] private bool _fillChests;
        [SerializeField] private bool _spawnAgentForTest;
        [SerializeField] private int _spawnAgentCount = 4;

        public static bool IsTesting { get; private set; }

        private ChestManager _chestManager;

        private void Start()
        {
            _chestManager = FindObjectOfType<ChestManager>();

            if(_spawnForTest) SpawnTestCannon();

            if (_fillChests) StartFillingChests();

            if (_spawnAgentForTest) SpawnTestBot();

            if (_spawnForTest || _fillChests || _spawnAgentForTest)
                IsTesting = true;

        }

        private void StartFillingChests()
        {
            _chestManager.StartFillChests();
        }

        private void SpawnTestCannon()
        {
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            spawnManager.SpawnPlayerManager();

            if (GameManager.Instance.useAndroidControllers)
                UIManager.Show<JoystickView>();

            FindObjectOfType<Cannon>().CanDoAction = true;
        }

        private void SpawnTestBot()
        {
            SaveManager.SetValue<int>("playWithBots", 1);

            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            spawnManager.TEST_SpawnBot(_spawnAgentCount);
        }

        private void Update()
        {
            if (!_spawnForTest)
                return;

            if(_fillChests)
                _chestManager.CheckRefillTimes();

        }

    }
}
