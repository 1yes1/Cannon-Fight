using CannonFightBase;
using CannonFightExtensions;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CannonFightUI
{
    public class NicknameView : UIView
    {
        public event Action<string> OnNicknameEnteredEvent;

        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private Button _okButton;

        private AnimationSettings _animationSettings;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(AnimationSettings animationSettings,SignalBus signalBus)
        {
            _animationSettings = animationSettings;
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            _okButton.onClick.AddListener(() =>
            {
                CheckNickname();
                _signalBus.Fire<OnButtonClickSignal>();
            });
        }

        public override void Show()
        {
            base.Show();
            transform.DOScale(_animationSettings.Show,true);
        }

        private void CheckNickname()
        {
            if(_text.text.Length < 5)
            {
                UIManager.Show<WarningView>(false,true).CreateWarning("NICKNAME MUST BE AT LEAST 4 CHARACTER!");
                return;
            }
            transform.DOScale(_animationSettings.Hide);

            OnNicknameEnteredEvent?.Invoke(_text.text);
        }

        public override void AddSubViews()
        {
        }

        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings Show;
            public TweenSettings Hide;
        }

    }
}
