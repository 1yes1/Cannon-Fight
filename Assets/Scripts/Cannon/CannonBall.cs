using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CannonFightBase
{
    public class CannonBall:MonoBehaviour
    {
        private float _damage;

        private Player _ownerPlayer;
        private Cannon _ownerCannon;

        public void Initialize(float damage,Cannon cannon,Player player)
        {
            _damage = damage;
            _ownerCannon = cannon;
            _ownerPlayer = player;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_ownerCannon == collision.gameObject.GetComponent<Cannon>())
                return;

            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_damage, collision.contacts[0].point, _ownerPlayer);

            collision.gameObject.GetComponent<IHittable>()?.OnHit(collision.contacts[0].point);

            //print(collision.gameObject.name);
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}