using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class RPCMediator : MonoBehaviour
    {
        private Dictionary<byte, RpcDelegate> _values;
        public bool dene = false;
        public void AddToRPC(byte key, RpcDelegate rpcDelegate)
        {
            if(_values == null)
                _values = new Dictionary<byte, RpcDelegate>();

            dene = true;
            if (_values.ContainsKey(key))
            {
                Debug.LogWarning("Keys cannot be the same!");
                return;
            }
            _values.Add(key, rpcDelegate);
        }

        //[PunRPC]
        //public void RPC_StartFire(Vector3 ballPosition, Vector3 ballDirection, float fireRange, int fireDamage, PhotonMessageInfo info)
        //{
        //    if (_fireController.Cannon.OwnPhotonView.IsMine)
        //        return;

        //    Player owner = info.Sender;

        //    //CannonBall cannonBall = _cannonBallFactory.Create();
        //    //cannonBall.Initialize(fireDamage,_cannon, owner);
        //    _fireController.Fire(owner, ballPosition, ballDirection, fireRange, fireDamage);

        //    //cannonBall.transform.position = _ballSpawnPoint.position;
        //    //cannonBall.transform.rotation = _ballSpawnPoint.rotation;

        //    //ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.fireCannonBallParticle, _ballSpawnPoint, ballPosition);

        //    //Burada diðer kullanýcýlarýn toplarý oluþturulduðu için bu toplar bize hasar vermeli. Yani own deðil normal cannonball olmalý
        //    //LayerMask layerMask = LayerMask.NameToLayer("CannonBall");
        //    //cannonBall.gameObject.layer = layerMask;

        //    //Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
        //    //rigidbody.AddForce(ballDirection * fireRange, ForceMode.Impulse);
        //}


        [PunRPC]
        public void RpcForwarder(byte key, object[] parameters, PhotonMessageInfo info)
        {
            if (_values == null)
                print("Heeey Ben null");

            if (_values.TryGetValue(key, out RpcDelegate method))
            {
                method(parameters,info);
            }
            else
            {
                Debug.LogWarning("No RPC key found!");
            }
        }

    }

}
