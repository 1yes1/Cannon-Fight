using CannonFightExtensions;
using CannonFightUI;
using DG.Tweening;
using GooglePlayGames;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

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
            AddPlayerItem();
            CheckForStartingGame();

        }

        private void AddPlayerItem()
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                UIManager.GetView<GameLoadingView>().AddOfflinePlayerItem();
                return;
            }

            foreach (Player item in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (item.IsLocal)
                {
                    AddPlayerItemWithPhotos(item);
                }
                else
                    UIManager.GetView<GameLoadingView>().AddPlayerItem(item);
            }

        }

        private void AddPlayerItemWithPhotos(Player player)
        {
            StartCoroutine(DownloadImage(player,PlayGamesPlatform.Instance.GetUserImageUrl()));
        }

        private IEnumerator DownloadImage(Player player,string MediaUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                UIManager.GetView<GameLoadingView>().AddPlayerItem(player);//Fotoyu çekemedik normal ekle
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                UIManager.GetView<GameLoadingView>().AddPlayerItem(player, sprite);
            }
        }

        private void CheckForStartingGame()
        {
            if (CloudSaveManager.GetValue<int>("playWithBots") == 1)
            {
                UIManager.GetView<GameLoadingView>().AddBotPlayerItem(_roomServerSettings.PlayersInGame - 1);
                Invoke(nameof(StartOfflineCountdown), _gameServerSettings.WaitAfterAllPlayersEnteredToGame);
                return;
            }

            if (!PhotonNetwork.IsMasterClient)
                return;

            Invoke(nameof(StartCountdown), _gameServerSettings.WaitAfterAllPlayersEnteredToGame);
            
        }

        private void StartOfflineCountdown()
        {
            GameEventCaller.Instance.OnGameReadyToStart();
            Invoke(nameof(StartGame), (int)_gameServerSettings.GameStartCountdown);
            UIManager.Show<CountdownView>().StartCountdown((int)_gameServerSettings.GameStartCountdown);
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
        }

        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings CameraRotating;
        }


    }
}
