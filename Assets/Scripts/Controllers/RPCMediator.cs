using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class RPCMediator : MonoBehaviour
    {
        private Dictionary<byte, IRpcMediator> _values;
        public bool dene = false;
        public void AddToRPC(byte key, IRpcMediator rpcMediator)
        {
            if(_values == null)
                _values = new Dictionary<byte, IRpcMediator>();

            dene = true;
            if (_values.ContainsKey(key))
            {
                Debug.LogWarning("Keys cannot be the same!");
                return;
            }
            _values.Add(key, rpcMediator);
        }


        [PunRPC]
        public void RpcForwarder(byte key, object[] parameters, PhotonMessageInfo info)
        {
            if (_values == null)
                Debug.LogWarning("_values is null");

            if (_values.TryGetValue(key, out IRpcMediator rpcMediator))
            {
                rpcMediator.RpcForwarder(parameters,info);
            }
            else
            {
                Debug.LogWarning("No RPC key found!");
            }
        }

    }

}
