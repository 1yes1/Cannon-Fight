using CannonFightUI;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CannonView : MonoBehaviour
    {
        [SerializeField] private GameObject _rotatorH;

        [SerializeField] private GameObject _rotatorV;

        [SerializeField] private Transform _cannonBallSpawnPoint;

        [SerializeField] private Transform _skillParticlePoint;

        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private PhotonView _photonView;

        [SerializeField] private MovementController.Settings _cannonControllerSettings;

        private Animation _animation;

        private Cannon _cannon;

        public MovementController.Settings CannonControllerSettings => _cannonControllerSettings;

        public Transform CannonBallSpawnPoint => _cannonBallSpawnPoint;

        public Transform SkillParticlePoint => _skillParticlePoint;

        public Rigidbody Rigidbody => _rigidbody;

        public PhotonView PhotonView => _photonView;



        [Inject]
        public void Construct(Cannon cannon)
        {
            _cannon = cannon;
        }


        private void OnEnable()
        {
            GameEventReceiver.OnPlayerFiredEvent += OnFire;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnPlayerFiredEvent -= OnFire;
        }

        private void Awake()
        {
            _animation = _rotatorV.GetComponent<Animation>();
        }

        public void GetRotators(out GameObject rotatorH, out GameObject rotatorV)
        {
            rotatorH = this._rotatorH;
            rotatorV = this._rotatorV;
        }

        private void OnFire()
        {
            if (!_cannon.OwnPhotonView.IsMine)
                return;

            if (_animation == null)
            {
                _animation = gameObject.GetComponent<Animation>();
            }

            _animation.Stop();
            _animation.Play();
        }

    }
}
