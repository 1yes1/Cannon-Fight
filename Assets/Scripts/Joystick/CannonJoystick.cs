using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class CannonJoystick : UISubView
    {
        public static CannonJoystick Instance;

        [SerializeField] private JoystickUICreator _joystickUICreator;

        [SerializeField] private RectTransform _canvasRectTransform;

        [SerializeField] private Image _knob;

        [Header("Joystick Controller")]

        [SerializeField] private float _range = 195;

        [SerializeField] private float _knobFrequency = 0.008f;

        private CanvasGroup _canvasGroup;

        private Vector3 _uiCreatorStartPosition;

        private RectTransform _uiCreatorRectTransform;

        private Vector3 _knobStartPosition;

        private Vector3 _knobClickAnchoredPosition;

        private Vector3 _startTouchPosition;

        private RectTransform _knobRectTransform;

        private float _horizontal;

        private float _vertical;

        public static float Horizontal => Instance._horizontal;

        public static float Vertical => Instance._vertical;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }


        public override void Initialize()
        {
            _knobRectTransform = _knob.GetComponent<RectTransform>();
            _uiCreatorRectTransform = _joystickUICreator.GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _uiCreatorStartPosition = _joystickUICreator.transform.position;
            _knobStartPosition = _knob.transform.position;
        }

        private void Update()
        {
            //print(IsPointerOverUIElement() ? "Over UI" : "Not over UI");

            if (!GameManager.Instance.useAndroidControllers)
                return;

            if (Input.touchCount == 0)
                return;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                bool isLeftSide = touch.position.x <= Screen.width / 2;

                if (!isLeftSide)
                    continue;

                if (touch.phase == TouchPhase.Began)
                {
                    _joystickUICreator.transform.position = touch.position;
                    _knob.transform.position = touch.position;

                    _knobClickAnchoredPosition = _knobRectTransform.anchoredPosition;
                    _startTouchPosition = touch.position;
                    _canvasGroup.alpha = 1;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 touchPos = touch.position;
                    float distanceY = touch.position.y - _startTouchPosition.y;

                    Vector3 pos = _knobRectTransform.anchoredPosition;
                    pos.x = Mathf.Clamp((touchPos / _canvasRectTransform.localScale.x).x, _knobClickAnchoredPosition.x - _range, _knobClickAnchoredPosition.x + _range);
                    _knobRectTransform.anchoredPosition = pos;

                    float changePercent = Mathf.Clamp(distanceY, -_joystickUICreator.MaxAmplitude, _joystickUICreator.MaxAmplitude);

                    //print(distanceY);
                    float amplitude = _joystickUICreator.ChangeAmplitude(changePercent);
                    _vertical = amplitude / 100f;

                    ChangeKnobPosition();
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    _joystickUICreator.transform.position = _uiCreatorStartPosition;
                    _knob.transform.position = _knobStartPosition;

                    _joystickUICreator.ChangeAmplitude(0);
                    _horizontal = 0;
                    _vertical = 0;
                    _canvasGroup.alpha = 0;
                }

            }


            #region Unity Editor
#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;

                bool isLeftSide = mousePos.x <= Screen.width / 2;

                if (!isLeftSide)
                    return;


                _joystickUICreator.transform.position = mousePos;
                _knob.transform.position = mousePos;

                _knobClickAnchoredPosition = _knobRectTransform.anchoredPosition;
                _startTouchPosition = mousePos;
                _canvasGroup.alpha = 1;
            }
            else if(Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;

                Vector3 touchPos = mousePos;
                float distanceY = mousePos.y - _startTouchPosition.y;

                Vector3 pos = _knobRectTransform.anchoredPosition;
                pos.x = Mathf.Clamp((touchPos / _canvasRectTransform.localScale.x).x, _knobClickAnchoredPosition.x - _range, _knobClickAnchoredPosition.x + _range);
                _knobRectTransform.anchoredPosition = pos;

                float changePercent = Mathf.Clamp(distanceY, -_joystickUICreator.MaxAmplitude, _joystickUICreator.MaxAmplitude);

                //print(distanceY);
                float amplitude = _joystickUICreator.ChangeAmplitude(changePercent);
                _vertical = amplitude / 100f;

                ChangeKnobPosition();

            }
            else if (Input.GetMouseButtonUp(0))
            {
                _joystickUICreator.transform.position = _uiCreatorStartPosition;
                _knob.transform.position = _knobStartPosition;

                _joystickUICreator.ChangeAmplitude(0);
                _horizontal = 0;
                _vertical = 0;
                _canvasGroup.alpha = 0;
            }


#endif
            #endregion


        }

        private void ChangeKnobPosition()
        {
            float posX = _knobRectTransform.anchoredPosition.x - _knobClickAnchoredPosition.x - _range;

            Vector3 pos = _knobRectTransform.anchoredPosition;
            pos.y = _knobClickAnchoredPosition.y - Mathf.Sin(posX * _knobFrequency) * _joystickUICreator.Amplitude;
            _knobRectTransform.anchoredPosition = pos;

            float width = GetWidth(_uiCreatorRectTransform);
            posX += width;
            float val = ((posX / width)) * 2 - 1f;

            _horizontal = val;
        }

        private float GetWidth(RectTransform rt)
        {
            var w = (rt.anchorMax.x - rt.anchorMin.x) * Screen.width + rt.sizeDelta.x;
            return w;
        }
    }
}
