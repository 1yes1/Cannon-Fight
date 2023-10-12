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

        private void Start()
        {

        }

        public void Initialize()
        {
            if (_photonView.IsMine)
            {
                SpawnCannon();
            }
        }

        public static PlayerManager Find(Player player)
        {
            return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x._photonView.Owner == player);
        }

        private void SpawnCannon()
        {
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


                PhotonNetwork.RaiseEvent(SpawnManager.SPAWN_CANNON_EVENT_CODE, data, raiseEventOptions, SendOptions.SendUnreliable);
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

            if (photonEvent.Code == SpawnManager.SPAWN_CANNON_EVENT_CODE)
            {
                object[] data = (object[])photonEvent.CustomData;

                Cannon cannon = _cannonFactory.Create();
                PhotonView photonView = cannon.GetComponent<PhotonView>();

                //print("---------GÝRDÝÝ: " + (int)data[2] + " PlayermANAGER: "+_photonView.ViewID);

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
            //Burada biz dahil herkese gönderiyoruz RpcTarget.All ile
            //Herkesin scripti çalýþýyor ve kendi managerlarýna kill i bildiriyorlar
            _photonView.RPC(nameof(RPC_OnDie), RpcTarget.All, attackerPlayer.ActorNumber,PhotonNetwork.LocalPlayer.ActorNumber);
        }

        [PunRPC]
        private void RPC_OnDie(int attackerPlayer,int deadPlayer)
        {
            Debug.LogError("----RPC_OnDie Çalýþtý Attacker RPC User Id: " + attackerPlayer + " Dead: "+deadPlayer);

            Player attacker = PhotonNetwork.CurrentRoom.GetPlayer(attackerPlayer);
            Player dead = PhotonNetwork.CurrentRoom.GetPlayer(deadPlayer);

            GameEventCaller.Instance.OnKill(attacker, dead);
            GameEventCaller.Instance.OnPlayerDie(dead);

            //Hashtable hashtable = new Hashtable();
            //hashtable.Add("isDead", true);
            //hashtable.Add("killerPlayer", attackerPlayer);
            //PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }


        public class Factory : PlaceholderFactory<PlayerManager>
        {
        }

    }
}
