using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public abstract class Character : MonoBehaviour
    {
        private CharacterManager _characterManager;
        
        private bool _isDead = false;

        private bool _canDoAction = false;

        public string NickName => _characterManager.NickName;

        public Sprite Picture => _characterManager.Picture;

        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        public bool CanDoAction
        {
            get => (_canDoAction && !_isDead);
            set => _canDoAction = value;    
        }

        private void OnEnable()
        {
            GameEventReceiver.OnGameStartedEvent += OnGameStarted;

        }

        private void OnDisable()
        {
            GameEventReceiver.OnGameStartedEvent -= OnGameStarted;

        }

        public void SetCharacterManager(CharacterManager characterManager)
        {
            _characterManager = characterManager;
        }

        public void GetKill()
        {
            _characterManager.GetKill();
        }

        private void OnGameStarted()
        {
            _canDoAction = true;
            OnGameStart();
        }

        protected virtual void OnGameStart() { }

    }
}
