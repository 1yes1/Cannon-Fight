using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class PlayerManager : MonoBehaviour
    {
        private Cannon.Factory _cannonFactory;

        private PhotonView _photonView;

        private Cannon _cannon;

        private float _killCount;

        public Cannon Cannon => _cannon;

        [Inject]
        public void Construct(Cannon.Factory factory)
        {
            _cannonFactory = factory;
        }

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += EVENT_SpawnCannon;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= EVENT_SpawnCannon;
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }


        public void Initialize()
        {
            if(_photonView.IsMine)
                SpawnCannon();
        }

        public static PlayerManager Find(Player player)
        {
            return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x._photonView.Owner == player);
        }

        private void SpawnCannon()
        {
            //print("----------------------Spawn Cannon---------------------");

            _cannon = _cannonFactory.Create();
            PhotonView photonView = _cannon.GetComponent<PhotonView>();
            
            SpawnPoint spawnPoint = SpawnManager.GetSpawnPoint();

            _cannon.transform.position = spawnPoint.Position;
            _cannon.transform.rotation = spawnPoint.Rotation;

            if (PhotonNetwork.AllocateViewID(photonView))
            {
                object[] data = new object[]
                {
                     spawnPoint.Position,spawnPoint.Rotation,photonView.ViewID
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                PhotonNetwork.RaiseEvent(EventCodeManager.SPAWN_CANNON_EVENT_CODE, data, raiseEventOptions, SendOptions.SendUnreliable);
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewId.");
                Destroy(_cannon);
            }

            foreach (var component in _cannon.GetComponents<ICannonBehaviour>())
                component.OnSpawn(this);

            GameEventCaller.Instance.OnBeforeOurPlayerSpawned();
        }

        private void EVENT_SpawnCannon(EventData photonEvent)
        {
            //Room da cache lediðimiz için bu kontrolü yapmamýz lazým
            //Eðer bu bilgisayar benim ise giren oyuncuyu birtek ben oluþturayým. 
            //Benim bilgisayarda oluþturulmuþ olan hali hazýrdaki diðer player manager lar buna karýþmasýn
            if (!_photonView.IsMine)
                return;

            if (photonEvent.Code == EventCodeManager.SPAWN_CANNON_EVENT_CODE)
            {
                object[] data = (object[])photonEvent.CustomData;

                Cannon cannon = _cannonFactory.Create();
                PhotonView photonView = cannon.GetComponent<PhotonView>();

                //print("----------------------Spawn Cannon Misafir---------------------");

                cannon.transform.position = (Vector3)data[0];
                cannon.transform.rotation = (Quaternion)data[1];
                photonView.ViewID = (int)data[2];
            }
        }

        public void GetKill()
        {
            _photonView.RPC(nameof(RPC_GetKill), _photonView.Owner);
        }

        [PunRPC]
        private void RPC_GetKill()
        {
            _killCount++;

            Hashtable hashtable = new Hashtable();
            hashtable.Add("killCount", _killCount);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }


        public void OnDie(Player attackerPlayer)
        {
            //Burada diðerlerine öldüðümüzü bildiriyoruz
            _photonView.RPC(nameof(RPC_OnDie), RpcTarget.Others, attackerPlayer.ActorNumber,PhotonNetwork.LocalPlayer.ActorNumber);

            GameEventCaller.Instance.OnOurPlayerDied(PhotonNetwork.LocalPlayer);

            //print("Biz Öldük: Dead:" + PhotonNetwork.LocalPlayer.NickName);

            PhotonNetwork.Destroy(_cannon.gameObject);

            Invoke(nameof(LeftRoom), 2);
        }

        [PunRPC]
        private void RPC_OnDie(int attackerPlayer,int deadPlayer)
        {
            //Birisi ölmüþ onun haberini alýyoruz. Yazýk olmuþ
            Player attacker = PhotonNetwork.CurrentRoom.GetPlayer(attackerPlayer);
            Player dead = PhotonNetwork.CurrentRoom.GetPlayer(deadPlayer);

            SetPlayerHashtables(dead);

            GameEventCaller.Instance.OnPlayerDied(dead);

            GameEventCaller.Instance.OnKill(attacker, dead);

            //print("Baþkasý Öldü: Attacker: " + attacker.NickName + " Dead:" + dead.NickName);
        }

        private void SetPlayerHashtables(Player player)
        {
            Hashtable hashtable = new Hashtable
            {
                { "isDead", true }
            };
            player.SetCustomProperties(hashtable);
        }


        private void LeftRoom()
        {
            PhotonNetwork.Disconnect();
        }
        

        public class Factory : PlaceholderFactory<PlayerManager>
        {
        }

    }
}
