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
        [SerializeField] private bool _spawnBotForTest;

        private ChestManager _chestManager;

        private void Start()
        {
            _chestManager = FindObjectOfType<ChestManager>();

            if(_spawnForTest)
            {
                SpawnTestCannon();
                StartFillingChests();
            }

            if (_spawnBotForTest)
                SpawnTestBot();

        }

        private void StartFillingChests()
        {
            _chestManager.StartFillChests();
        }

        private void SpawnTestCannon()
        {
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            spawnManager.SpawnPlayerManager();
        }

        private void SpawnTestBot()
        {
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            spawnManager.TEST_SpawnBot();
        }

        private void Update()
        {
            if (!_spawnForTest)
                return;

            _chestManager.CheckRefillTimes();

        }

    }
}
