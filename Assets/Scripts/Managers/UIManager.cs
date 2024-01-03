using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CannonFightBase;

namespace CannonFightUI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager _instance;

        [SerializeField] private UIView _startingView;

        [SerializeField] private UIView[] _views;

        private UIView _currentView;

        private readonly Stack<UIView> _history = new Stack<UIView>();

        private void OnEnable()
        {
            GameEventReceiver.OnGameStartedEvent += OnGameStarted;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnGameStartedEvent -= OnGameStarted;
        }

        private void Awake()
        {
            if(_instance == null) _instance = this;

            for (int i = 0; i < _views.Length; i++)
            {
                _views[i].Initialize();
                _views[i].Hide();
            }
            Show(_startingView, true);
            Show<CoinView>(false, true);
        }

        public static T GetView<T>() where T : UIView
        {
            for (int i = 0; i < _instance._views.Length; i++)
            {
                if (_instance._views[i] is T tView)
                {
                    return tView;
                }
            }

            return null;
        }

        public static T Show<T>(bool remember = true,bool isFixed = false) where T : UIView
        {
            for (int i = 0; i < _instance._views.Length; i++)
            {
                if (_instance._views[i] is T)
                {
                    if(_instance._currentView != null)
                    {
                        if(remember)
                        {
                            _instance._history.Push(_instance._currentView);
                        }

                        if(!isFixed)
                            _instance._currentView.Hide();
                    }

                    _instance._views[i].Show();

                    if (!isFixed)
                        _instance._currentView = _instance._views[i];

                    return _instance._views[i] as T;
                }
            }

            return null;

        }

        public static T ShowWithDelay<T>(float delay,bool remember = true, bool isPopup = false) where T : UIView
        {
            UIView view = GetView<T>();

            _instance.StartCoroutine(ShowDelay(view, delay));

            return (T)view;
        }

        private static IEnumerator ShowDelay(UIView view,float time)
        {
            yield return new WaitForSeconds(time);
            view.Show();
        }

        public static void Show(UIView view, bool remember = true)
        {
            if(_instance._currentView != null)
            {
                if(remember)
                {
                    _instance._history.Push(_instance._currentView);
                }

                _instance._currentView.Hide();
            }

            view.Show();

            _instance._currentView = view;
        }

        public static void ShowLast()
        {
            if( _instance._history.Count != 0)
            {
                Show(_instance._history.Pop(),false);
            }
        }


        private void OnGameStarted()
        {
            Show<GamePanelView>(true,true);
            Show<JoystickView>(false,true);
            Show<CrosshairView>(false,true);
        }

    }

}

