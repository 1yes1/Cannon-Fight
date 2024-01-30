using CannonFightUI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Zenject;

namespace CannonFightBase
{
    public class MainMenuManager : MonoBehaviour
    {
        private SignalBus _signalBus;

        [SerializeField] private bool TEST_passTutorial;

        private bool _isFirstOpening;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<OnGameMatchingStartedSignal>(OpenMatchingMenu);
            _signalBus.Subscribe<OnFirstOpeningSignal>(OnFirstOpening);
            Launcher.OnPhotonConnectResultEvent += OnPhotonConnectResult;
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<OnGameMatchingStartedSignal>(OpenMatchingMenu);
            _signalBus.Unsubscribe<OnFirstOpeningSignal>(OnFirstOpening);
            Launcher.OnPhotonConnectResultEvent -= OnPhotonConnectResult;
            UIManager.GetView<NicknameView>().OnNicknameEnteredEvent -= OnNicknameEntered;
        }

        private void Start()
        {
            if(RoomManager.IsFirstFight)
            {
                Launcher.Instance.StartFight();
                RoomManager.IsFirstFight = false;
            }

#if UNITY_EDITOR

            UnityEngine.Cursor.visible = true;
#endif
        }

        private void OnNicknameEntered(string nickname)
        {
            UserManager.Instance.SetNickname(nickname);
            _signalBus.Fire<OnNicknameEnteredSignal>();
        }

        private void OnPhotonConnectResult(bool isConnected)
        {
            if (_isFirstOpening && !TEST_passTutorial)
            {
                LoadSceneManager.LoadScene(GameScene.Tutorial);//Tutorial scene açýlsýn

            }
            else
            {
                OpenMainMenu();
            }
        }


        private void OpenMainMenu()
        {
            UIManager.Show<MainMenuView>(true);
            UIManager.Show<CoinView>(false, true);
            _signalBus.Fire(new OnMainMenuOpenedSignal());
        }

        private void OpenMatchingMenu()
        {
            UIManager.HideAllViews();
            UIManager.Show<MatchingMenuView>(true);
        }

        private void OnFirstOpening()
        {
            _isFirstOpening = true;

            //UIManager.HideAllViews();
            UIManager.Show<NicknameView>().OnNicknameEnteredEvent += OnNicknameEntered;
        }




    }
}
