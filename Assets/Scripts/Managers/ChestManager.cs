using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class ChestManager : MonoBehaviour
    {
        private PhotonView _photonView;

        private List<Chest> _chests;

        private Chest.Settings _chestSettings;

        private int _openedChestCount = 0;

        [Inject]
        public void Construct(Chest.Settings settings,List<Chest> chests)
        {
            _chestSettings = settings;
            _chests = chests;
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            //CreateStack();
        }

        private void OnEnable()
        {
            GameEventReceiver.OnChestOpenedEvent += OnChestOpened;
            if (!PhotonNetwork.IsMasterClient)
            {
            }
        }

        private void OnDisable()
        {
            GameEventReceiver.OnChestOpenedEvent -= OnChestOpened;
            if (!PhotonNetwork.IsMasterClient)
            {
            }
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
                Invoke(nameof(StartFillChests), _chestSettings.StartFillTime);

        }

        private void Update()
        {
            if(Time.frameCount % 10 == 0 && _openedChestCount > 0)
                CheckRefillTimes();
        }

        private void CheckRefillTimes()
        {
            for (int i = 0; i < _chests.Count; i++)
            {
                if (_chests[i].CanRefill())
                {
                    _openedChestCount--;
                    _chests[i].Refill();
                }
            }
        }

        private void OnChestOpened(Chest obj)
        {
            _openedChestCount++;
        }

        private void FillChest()
        {
            Chest chest = _chests.Find(x => !x.IsOpened && !x.IsFilled);
            if (chest == null)
            {
                CancelInvoke(nameof(FillChest));
                return;
            }
            chest.Refill();
            int chestIndex = _chests.IndexOf(chest);

            if (!PhotonNetwork.IsConnected)
                return;

            ChestStartedFillingEvent(chestIndex);
        }


        public void StartFillChests()
        {
            InvokeRepeating(nameof(FillChest), _chestSettings.StartFillFrequency, _chestSettings.StartFillFrequency);
        }



        #region Events&RPCs

        private void ChestStartedFillingEvent(int chestIndex)
        {
            _photonView.RPC(nameof(RPC_ChestStartedFilling), RpcTarget.Others, chestIndex);
        }

        [PunRPC]
        private void RPC_ChestStartedFilling(int chestIndex)
        {
            _chests[chestIndex].Refill();
        }

        #endregion


    }
}
