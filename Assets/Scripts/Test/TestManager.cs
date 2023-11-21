using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TestManager : MonoBehaviour
    {
        [SerializeField] private bool _spawnForTest;

        private ChestManager _chestManager;

        private void Start()
        {
            if (!_spawnForTest)
                return;

            _chestManager = FindObjectOfType<ChestManager>();

            SpawnTestCannon();
            StartFillingChests();
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

        private void Update()
        {
            if (!_spawnForTest)
                return;

            _chestManager.CheckRefillTimes();

        }

    }
}
