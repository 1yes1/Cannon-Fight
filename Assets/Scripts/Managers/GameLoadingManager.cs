using CannonFightUI;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEditor.Progress;

namespace CannonFightBase
{
    public class GameLoadingManager : MonoBehaviour
    {
        private RoomManager.GameServerSettings _gameServerSettings;

        private RoomManager.RoomServerSettings _roomServerSettings;

        private AnimationSettings _animationSettings;

        private PhotonView _photonView;

        private MainCamera _camera;

        private GameObject _cameraParent;

        [Inject]
        public void Construct(MainCamera camera,AnimationSettings animationSettings,RoomManager.GameServerSettings gameServerSettings,RoomManager.RoomServerSettings roomServerSettings)
        {
            _camera = camera;
            _animationSettings = animationSettings;
            _gameServerSettings = gameServerSettings;
            _roomServerSettings = roomServerSettings;
        }

        private void OnEnable()
        {
            GameEventReceiver.OnGameSceneLoadedEvent += OnGameSceneLoaded;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnGameSceneLoadedEvent -= OnGameSceneLoaded;
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            //OnGameSceneLoaded();
        }

        private void OnGameSceneLoaded()
        {
            _cameraParent = new GameObject("Camera Parent");
            _cameraParent.transform.position = Vector3.zero;
            _camera.transform.SetParent(_cameraParent.transform, true);
            _cameraParent.transform.DORotate(_animationSettings.CameraRotating).SetLoops(-1).Play();

            UIManager.Show<GameLoadingView>();

            //print("Custom Properties:" + SaveManager.GetValue<int>("playWithBots"));
            AddPlayerItems();
            CheckForStartingGame();

        }

        private void AddPlayerItems()
        {
            foreach (Player item in PhotonNetwork.CurrentRoom.Players.Values)
            {
                UIManager.GetView<GameLoadingView>().AddPlayerItem(item);
            }
        }

        private void CheckForStartingGame()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            if (PhotonNetwork.CurrentRoom.PlayerCount == _roomServerSettings.PlayersInGame)
            {
                Invoke(nameof(StartCountdown), _gameServerSettings.WaitAfterAllPlayersEnteredToGame);
            }
            else if(SaveManager.GetValue<int>("playWithBots") == 1)
            {

                //Burada botlar doðacak
                UIManager.GetView<GameLoadingView>().AddBotPlayerItem(_roomServerSettings.PlayersInGame - 1);
                Invoke(nameof(StartCountdown), _gameServerSettings.WaitAfterAllPlayersEnteredToGame);

            }
        }

        private void StartCountdown()
        {

            //Eðer Others yapýp OnGameReadyToStart eventini burada çalýþtýrýrsak her bilgisayarda önce master player manager ý oluþturuluyor ve bu da Raiseeventlerde sýkýntý çýkarýyor. Rakibin cannon unu oluþturmuyor
            //Ondan dolayý GameEventCaller.Instance.OnGameReadyToStart(); kýsmý herkese ayný anda gitmeli
            _photonView.RPC(nameof(RPC_StartCountdown), RpcTarget.All);
        }

        [PunRPC]
        private void RPC_StartCountdown()
        {
            GameEventCaller.Instance.OnGameReadyToStart();
            Invoke(nameof(StartGame), (int)_gameServerSettings.GameStartCountdown);
            UIManager.Show<CountdownView>().StartCountdown((int)_gameServerSettings.GameStartCountdown);
        }

        private void StartGame()
        {
            GameEventCaller.Instance.OnGameStarted();
            _photonView.RPC(nameof(RPC_StartGame), RpcTarget.Others);
        }

        [PunRPC]
        private void RPC_StartGame()
        {
            GameEventCaller.Instance.OnGameStarted();
        }

        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings CameraRotating;
        }


    }
}
