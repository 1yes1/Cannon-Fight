using CartoonFX;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CannonDamageHandler: IDamageable,IInitializable, IRpcMediator
    {
        private ParticleSettings _particleSettings;

        private TakeDamageParticle.Factory _takeDamageParticleFactory;

        private Cannon _cannon;

        private RPCMediator _rpcMediator;

        private int _health = 100;

        private bool _isDead = false;

        private const byte RPC_DAMAGE_PARTICLE = 2;


        public bool IsDead => _isDead;

        public int Health
        {
            get { return _health; }
            set
            {
                _health = (value < 0) ? 0 : value;

                if (_cannon.OwnPhotonView.IsMine)
                    GameEventCaller.Instance.OnOurPlayerHealthChanged(_health);
            }
        }
        public CannonDamageHandler(Cannon cannon, ParticleSettings particleSettings, RPCMediator rpcMediator, TakeDamageParticle.Factory takeDamageParticleFactory)
        {
            _cannon = cannon;
            _particleSettings = particleSettings;
            _rpcMediator = rpcMediator;
            _takeDamageParticleFactory = takeDamageParticleFactory;
        }

        public void Initialize()
        {
            _rpcMediator.AddToRPC(RPC_DAMAGE_PARTICLE, this);
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer)
        {
            TakeDamageParticle particleSystem = ParticleManager.CreateWithFactory<TakeDamageParticle>(_takeDamageParticleFactory, hitPoint, null, false);

            //Take Damage i çalýþtýrmasý gereken bilgisayar bu bilgisayar deðilse devam etme. Particle oluþturmak yeterli
            if (!_cannon.OwnPhotonView.IsMine)
                return;

            Health -= damage;

            particleSystem.GetComponent<CFXR_Effect>().enabled = true;
            particleSystem.GetComponent<CFXR_Effect>().cameraShake.enabled = true;

            if (Health <= 0)
            {
                Die(attackerPlayer);
            }

            _cannon.OwnPhotonView.RPC(nameof(_rpcMediator.RpcForwarder), RpcTarget.Others, RPC_DAMAGE_PARTICLE, new object[] { hitPoint });
        }


        public void RpcForwarder(object[] objects, PhotonMessageInfo info)
        {
            Vector3 hitPoint = (Vector3)objects[0];
            RunDamageParticle(hitPoint);
        }

        private void RunDamageParticle(Vector3 hitPoint)
        {
            ParticleManager.CreateAndPlay(_particleSettings.TakeDamageParticle, null, hitPoint, false);
        }


        private void Die(Player attackerPlayer)
        {
            //if (!_photonView.IsMine)
            //    return;
            if (_isDead)
                return;

            _cannon.Die(attackerPlayer);

            _isDead = true;
        }



        [Serializable]
        public class ParticleSettings
        {
            public ParticleSystem TakeDamageParticle;
        }

    }
}
