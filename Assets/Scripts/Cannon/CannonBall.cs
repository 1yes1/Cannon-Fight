﻿using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;
using Player = Photon.Realtime.Player;

namespace CannonFightBase
{
    public class CannonBall:MonoBehaviour, IPoolable<IMemoryPool>
    {
        private int _damage;

        private IMemoryPool _pool;

        private Player _ownerPlayer;

        private CannonBase _ownerCannon;

        private Rigidbody _rigidbody;

        public void Initialize(int damage, CannonBase cannon,Player player)
        {
            _damage = damage;
            _ownerCannon = cannon;
            _ownerPlayer = player;
        }

        public void Initialize(int damage, CannonBase cannon)
        {
            _damage = damage;
            _ownerCannon = cannon;
        }

        public void OnDespawned()
        {
            //print("Despawn");
            //transform.position = _startPosition;
            //transform.rotation = _startRotation;
            ResetRigidbody();
        }

        public void OnSpawned(IMemoryPool p3)
        {
            _pool = p3;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_ownerCannon.GetComponent<CannonBase>() == collision.gameObject.GetComponent<CannonBase>() || _ownerCannon == null)
                return;

            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_damage, collision.contacts[0].point, _ownerPlayer,_ownerCannon);

            collision.gameObject.GetComponent<IHittable>()?.OnHit(collision.contacts[0].point);

            //print(collision.gameObject.name);
            //gameObject.SetActive(false);
            _pool.Despawn(this);
            //Destroy(gameObject);
        }

        private void ResetRigidbody()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void SetLayer(int layer)
        {
            gameObject.layer = layer;
        }

        public class Factory : PlaceholderFactory<CannonBall>
        {

        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, CannonBall>
        {

        }

    }
}