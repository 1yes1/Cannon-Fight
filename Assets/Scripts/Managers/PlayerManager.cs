using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
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
                component.OnSpawn();

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

    }
}
