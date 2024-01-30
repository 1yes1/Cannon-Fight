using CannonFightUI;
#if UNITY_ANDROID
using GooglePlayGames;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace CannonFightBase
{
    public class ProfileImagePresenter : PresenterBase
    {
        private ProfileImageSubView _profileImageSubView;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnEnable()
        {
            _signalBus.Subscribe<OnPlayGamesAuthanticated>(SetProfilePicture);
            _signalBus.Subscribe<OnMainMenuOpenedSignal>(OnMainMenuOpened);
        }

        public void OnDisable()
        {
            _signalBus.Unsubscribe<OnPlayGamesAuthanticated>(SetProfilePicture);
            _signalBus.Unsubscribe<OnMainMenuOpenedSignal>(OnMainMenuOpened);
        }


        public void Start()
        {
            _profileImageSubView = UIManager.GetSubView<ProfileImageSubView>();
        }

        private void SetProfilePicture(OnPlayGamesAuthanticated onPlayGamesAuthanticated)
        {
#if UNITY_ANDROID
            if(onPlayGamesAuthanticated.IsAuthanticated)
                StartCoroutine(DownloadImage(PlayGamesPlatform.Instance.GetUserImageUrl()));
#endif
        }

        private void OnMainMenuOpened()
        {
            if(_profileImageSubView == null)
                _profileImageSubView = UIManager.GetSubView<ProfileImageSubView>();

            _profileImageSubView.SetNickname(CloudSaveManager.GetValue<string>(UserManager.NICKNAME_PREFS));
        }

        private IEnumerator DownloadImage(string MediaUrl)
        {
            print("Requesterrrrrr");

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                _profileImageSubView.SetParameters(((DownloadHandlerTexture)request.downloadHandler).texture);
                _profileImageSubView.SetProfileImage();
            }
        }


    }
}
