using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CannonFightBase
{
    internal interface IDamageable
    {
        public void TakeDamage(float damage,Vector3 hitPoint,Player attackerPlayer);
    }
}
