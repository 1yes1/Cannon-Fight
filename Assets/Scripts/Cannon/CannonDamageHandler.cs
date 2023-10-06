using CartoonFX;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class CannonDamageHandler: IDamageable
    {
        private ParticleSettings _particleSettings;

        private Cannon _cannon;

        private int _health = 100;

        private bool _isDead = false; 

        public bool IsDead => _isDead;

        public int Health
        {
            get { return _health; }
            set
            {
                _health = (value < 0) ? 0 : value;

                //if (_photonView.IsMine)
                GameEventCaller.Instance.OnOurPlayerHealthChanged();
            }
        }
        public CannonDamageHandler(Cannon cannon, ParticleSettings particleSettings)
        {
            _cannon = cannon;
            _particleSettings = particleSettings;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer)
        {
            _health -= damage;

            ParticleSystem particleSystem = ParticleManager.CreateAndPlay(_particleSettings.TakeDamageParticle, _cannon.transform, hitPoint, false);
            particleSystem.GetComponent<CFXR_Effect>().cameraShake.enabled = true;

            if (Health <= 0)
            {
                Die(attackerPlayer);
            }

            _cannon.OwnPhotonView.RPC(nameof(RPC_RunDamageParticle), RpcTarget.All, hitPoint);
        }




        [PunRPC]
        private void RPC_RunDamageParticle(Vector3 hitPoint)
        {
            if (_cannon.OwnPhotonView.IsMine)
                return;

            //print(" Alaannnn: " + PhotonNetwork.NickName);

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
