using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public interface IRpcMediator
    {
        public void RpcForwarder(object[] objects, PhotonMessageInfo info);
    }
}
