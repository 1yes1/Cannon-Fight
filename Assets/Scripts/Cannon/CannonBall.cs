using Photon.Pun;
using Photon.Realtime;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;
using Player = Photon.Realtime.Player;

namespace CannonFightBase
{
    public class CannonBall:MonoBehaviour, IPoolable<IMemoryPool>,IDisposable
    {
        private int _damage;

        private IMemoryPool _pool;

        private Player _ownerPlayer;

        private Character _ownerCannon;

        private Rigidbody _rigidbody;

        public void Initialize(int damage, Character cannon,Player player)
        {
            _damage = damage;
            _ownerCannon = cannon;
            _ownerPlayer = player;
        }

        public void Initialize(int damage, Character cannon)
        {
            _damage = damage;
            _ownerCannon = cannon;
            _ownerPlayer = null;
        }

        public void Dispose()
        {
            if (_pool != null)
                _pool.Despawn(this);
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
            if (collision.gameObject == null || _ownerCannon == null || _ownerCannon.GetComponent<Character>() == collision.gameObject.GetComponent<Character>())
                return;

            //Vector3 position = collision.contacts[0].point;
            Vector3 position = transform.position;


            if(collision.gameObject.TryGetComponent<IHittable>(out IHittable hittable))
            {
                hittable.OnHit(position);
                AudioManager.PlaySound(GameSound.WallHit, position);
            }
            else
            {
                collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_damage, position, _ownerPlayer, _ownerCannon);
            }


            //Debug.Log("Değdi: " + collision.gameObject.name);
            //Debug.Log("Nokta: " + position);
            Dispose();
            //print(collision.gameObject.name);
            //gameObject.SetActive(false);
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