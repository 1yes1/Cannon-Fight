using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace CannonFightBase
{
    public class PlayerManager : MonoBehaviour
    {
        private PhotonView _photonView;

        private SpawnPoint _spawnPoint;

        private Cannon _cannon;

        private float _killCount;


        public Cannon Cannon => _cannon;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            if (_photonView.IsMine)
            {
                CreateCannonPlayer();
            }
        }

        public void Initialize(SpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }

        public static PlayerManager Find(Player player)
        {
            return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x._photonView.Owner == player);
        }

        private void CreateCannonPlayer()
        {
            _cannon = PhotonNetwork.Instantiate("Cannon", _spawnPoint.Position, _spawnPoint.Rotation).GetComponent<Cannon>();
            foreach (var component in _cannon.GetComponents<ICannonBehaviour>())
                component.OnSpawn(this);

            GameEventCaller.Instance.BeforeOurPlayerSpawned();
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
    }
}
