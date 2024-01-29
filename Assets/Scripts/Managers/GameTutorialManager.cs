using CannonFightUI;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CannonFightBase
{
    public class GameTutorialManager : MonoBehaviour
    {
        private List<TutorialText> _tutorialTexts;

        [SerializeField] private int _tutorialIndex;
        
        private TutorialView _view;

        private TutorialEventController _controller;

        [Inject]
        private void Construct(GameTutorialSettings gameTutorialSettings,TutorialEventController tutorialEventController)
        {
            _tutorialTexts = gameTutorialSettings.TutorialTexts;
            _controller = tutorialEventController;

        }

        private void OnEnable()
        {
            _controller.OnTutorialCompletedEvent += OnTutorialEventCompleted;
        }

        private void OnDisable()
        {
            _controller.OnTutorialCompletedEvent -= OnTutorialEventCompleted;
        }

        private void Awake()
        {
        }


        private void Start()
        {
            _view = UIManager.GetView<TutorialView>();
            CloudSaveManager.SetValue<int>("isTutorial", 1);
            TutorialActivator.SpawnCannon();
            MoveNextTutorialEvent();

            UIManager.Show<JoystickView>(false,true);
            UIManager.Show<CrosshairView>(false,true);
            UIManager.GetSubView<AimFireJoystick>().Hide();
        }

        private void MoveNextTutorialEvent()
        {
            TutorialEventType tutorialEventType = (TutorialEventType)(_tutorialIndex);

            //Bir önceki varsa onlarý kaldýrýyoruz
            if (_tutorialIndex != 0)
                TutorialActivator.SetActiveTutorialEvent((TutorialEventType)(_tutorialIndex - 1),false);

            TutorialActivator.SetActiveTutorialEvent(tutorialEventType,true);
            UIManager.Show<TutorialView>(false,true);

            TutorialText tutorialText = _tutorialTexts.Find(x => x.Type == ((TutorialEventType)_tutorialIndex));
            _view.SetText(tutorialText.Text);

            _controller.StartNewTutorialEvent(tutorialEventType,GetParameters(tutorialEventType));

            _tutorialIndex++;

            if (_tutorialIndex >= _tutorialTexts.Count)
            {
                Invoke(nameof(AllTutorialEventsCompleted),2);
                return;
            }

            if (tutorialEventType == TutorialEventType.Hi)
                Invoke(nameof(OnTutorialEventCompletedWithTime),1);
        }

        private void AllTutorialEventsCompleted()
        {
            LoadSceneManager.LoadScene(GameScene.Menu);
            CloudSaveManager.SetValue<int>("goFirstFight", 1);
        }

        public void OnTutorialEventCompleted()
        {
            _view.SetCompleted();
            Invoke(nameof(MoveNextTutorialEvent),1.25f);
        }

        public void OnTutorialEventCompletedWithTime()
        {
            print("Completed With Time");
            //_view.SetCompleted();
            Invoke(nameof(MoveNextTutorialEvent),1.25f);
        }

        private object[] GetParameters(TutorialEventType tutorialEventType)
        {
            return TutorialActivator.GetTutorialParameters(tutorialEventType);
        }

        [Serializable]
        public struct GameTutorialSettings
        {
            public List<TutorialText> TutorialTexts;
        }

    }
    public enum TutorialEventType
    {
        Hi,
        Move,
        Aim,
        KillEnemy,
        OpenChestDamage,
        DamagePotionKill,
        OpenChestMultiball,
        MultiballPotionKill,
        OpenChestHealth,
        Done,
    }

    [Serializable]
    public struct TutorialText
    {
        public TutorialEventType Type;
        [TextArea]
        public string Text;
    }
}
