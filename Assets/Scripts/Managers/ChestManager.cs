using ExitGames.Client.Photon;
using ModestTree;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CannonFightBase
{
    public class ChestManager : MonoBehaviour
    {
        private int _openedChestCount = 0;

        private PhotonView PhotonView { get; set; }

        private List<Chest> Chests { get;  set; }

        private Chest.Settings ChestSettings { get; set; }

        private Potion.Settings PotionSettings { get; set; }


        [Inject]
        public void Construct(Chest.Settings settings,List<Chest> chests, Potion.Settings potionSettings)
        {
            ChestSettings = settings;
            Chests = chests;
            PotionSettings = potionSettings;
        }

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            //CreateStack();
        }

        private void OnEnable()
        {
            GameEventReceiver.OnChestOpenedEvent += OnChestOpened;
            if (!PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.NetworkingClient.EventReceived += EVENT_ChestFilled;
            }
        }

        private void OnDisable()
        {
            GameEventReceiver.OnChestOpenedEvent -= OnChestOpened;
            if (!PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.NetworkingClient.EventReceived -= EVENT_ChestFilled;
            }
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
                Invoke(nameof(StartFillChests), ChestSettings.StartFillTime);

        }

        private void Update()
        {
            if(Time.frameCount % 10 == 0 && _openedChestCount > 0 && PhotonNetwork.IsMasterClient)
                CheckRefillTimes();
        }

        internal void CheckRefillTimes()
        {
            //Refill için de random ayarlama olcak master ile ve senkronize edilecek
            for (int i = 0; i < Chests.Count; i++)
            {
                if (Chests[i].CanRefill())
                {
                    _openedChestCount--;
                    int potionIndex = GetRandomPotionTypeIndex();
                    int chestIndex = i;
                    Chests[chestIndex].Refill(potionIndex);

                    ChestStartedFillingRpc(chestIndex, potionIndex);
                    ChestFilledEvent(chestIndex, potionIndex);
                }
            }
        }

        private void OnChestOpened(Chest obj)
        {
            _openedChestCount++;
        }

        public void StartFillChests()
        {
            InvokeRepeating(nameof(FillChest), ChestSettings.StartFillFrequency, ChestSettings.StartFillFrequency);
        }


        private void FillChest()
        {
            Chest chest = Chests.Find(x => !x.IsOpened && !x.IsFilled);
            if (chest == null)
            {
                CancelInvoke(nameof(FillChest));
                return;
            }

            int potionIndex = GetRandomPotionTypeIndex();

            chest.Refill(potionIndex);

            int chestIndex = Chests.IndexOf(chest);

            if (!PhotonNetwork.IsConnected)
                return;

            ChestStartedFillingRpc(chestIndex, potionIndex);
            ChestFilledEvent(chestIndex, potionIndex);



        }


        #region Events&RPCs

        private void ChestStartedFillingRpc(int chestIndex,int potionIndex)
        {
            PhotonView.RPC(nameof(RPC_ChestStartedFilling), RpcTarget.Others, chestIndex, potionIndex);
        }

        [PunRPC]
        private void RPC_ChestStartedFilling(int chestIndex,int potionIndex)
        {
            Chests[chestIndex].Refill(potionIndex);
        }

        //Þimdilik kalsýn fakat her oyuncu ayný anda gireceði için gerek yok muhtemlen
        private void ChestFilledEvent(int chestIndex, int potionIndex)
        {
            object[] data = new object[]
            {
                chestIndex,potionIndex
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };

            PhotonNetwork.RaiseEvent(EventCodeManager.CHEST_FILLED_EVENT_CODE, data, raiseEventOptions, SendOptions.SendUnreliable);
        }


        private void EVENT_ChestFilled(EventData photonEvent)
        {
            if (photonEvent.Code == EventCodeManager.CHEST_FILLED_EVENT_CODE)
            {
                object[] data = (object[])photonEvent.CustomData;
                int chestIndex = (int)data[0];
                int potionIndex = (int)data[1];

                Chests[chestIndex].Refill(potionIndex);
            }

        }

        #endregion


        private int GetRandomPotionTypeIndex()
        {
            int rnd = Random.Range(0, PotionSettings.PotionTypes.Count);

            return rnd;
        }


    }
}
