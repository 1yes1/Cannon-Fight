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
        private static UIManager _instance;

        [SerializeField] private UIView _startingView;

        [SerializeField] private UIView[] _views;

        private List<Tuple<UIView, UISubView>> _subViewTuples;

        private List<UISubView> _subViews;

        private UIView _currentView;

        private readonly Stack<UIView> _history = new Stack<UIView>();

        public static UIManager Instance => _instance;

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

            _subViewTuples = new List<Tuple<UIView, UISubView>>();
            _subViews = new List<UISubView>();

            for (int i = 0; i < _views.Length; i++)
            {
                _views[i].Initialize();
                _views[i].AddSubViews();
                _views[i].HideImmediately();
            }
            Show(_startingView, true);
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

        private static void Show(UIView view, bool remember = true)
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

        public static void HideAllViews()
        {
            for (int i = 0; i < _instance._views.Length; i++)
            {
                _instance._views[i].Hide();
            }
        }

        public static void AddSubView(UIView uiView,UISubView subView)
        {
            _instance._subViewTuples.Add(Tuple.Create(uiView, subView));
            _instance._subViews.Add(subView);
        }

        public static TUISubView GetSubView<TUIView, TUISubView>() where TUIView : UIView where TUISubView : UISubView
        {
            for (int i = 0; i < _instance._subViewTuples.Count; i++)
            {
                foreach (Tuple<UIView, UISubView> item in _instance._subViewTuples)
                {
                    if(item.Item1 is TUIView && item.Item2 is TUISubView)
                    {
                        return (TUISubView)item.Item2;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// If there are multiple UIView that use same UISubViews, use other overload
        /// </summary>
        /// <typeparam name="TUISubView"></typeparam>
        /// <returns></returns>
        public static TUISubView GetSubView<TUISubView>() where TUISubView : UISubView
        {
            for (int i = 0; i < _instance._subViews.Count; i++)
            {
                if (_instance._subViews[i] is TUISubView)
                {
                    return (TUISubView)_instance._subViews[i];
                }
            }
            return null;
        }



        private void OnGameStarted()
        {
            Show<GamePanelView>(true,true);
            Show<JoystickView>(false,true);
            Show<CrosshairView>(false,true);
        }

    }

}

