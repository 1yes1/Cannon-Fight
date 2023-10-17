using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using Zenject;
using static UnityEngine.UI.Image;

namespace CannonFightBase
{
    public class AimController : ITickable
    {
        private Settings _settings;

        private Cannon _cannon;

        private CannonView _cannonView;

        private GameObject _rotatorH;

        private GameObject _rotatorV;

        private Vector3 _lastMousePosition;

        private float _angleHor;

        private float _angleVer;

        private Transform _transform;

        public AimController(Cannon cannon, CannonView cannonView, Settings settings)
        {
            _cannon = cannon;
            _settings = settings;
            _transform = cannon.transform;
            _cannonView = cannonView;
            //if (!_photonView.IsMine)
            //    return;
            Debug.Log("AimController Y�klendir");
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            _cannonView.GetRotators(out _rotatorH, out _rotatorV);
            _lastMousePosition = Input.mousePosition;
        }


        public void Tick()
        {
            //return;
            if (!_cannon.OwnPhotonView.IsMine)
                return;

            if (_cannon.IsDead)
                return;

            if (Application.isFocused)
                UnityEngine.Cursor.visible = false;
            else
                return;

            if (GameManager.Instance.useAndroidControllers)
                SetMobileController();
            else
                SetPcController();

            //RotateHorizontal();
            //RotateVertical();
        }

        private void SetMobileController()
        {
            if (Input.touchCount == 0)
                return;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);//Input Manager da ayarlanmal�
                bool isRightSide = touch.position.x > Screen.width / 2;

                if (!isRightSide)
                    continue;

                RotateCannon(touch.deltaPosition);
            }
        }

        private void SetPcController()
        {
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            RotateCannon(delta);
        }

        private void RotateCannon(Vector3 delta)
        {
            float hor = delta.x * _settings.HorizontalRotSpeed * Time.deltaTime;
            hor = (Mathf.Abs(delta.x) < 0.25f) ? 0 : hor;
            float ver = delta.y * _settings.VerticalRotSpeed * Time.deltaTime;
            ver = (Mathf.Abs(delta.y) < 0.25f) ? 0 : ver;

            _angleHor += hor;
            _angleHor = Mathf.Clamp(_angleHor, -_settings.MaxHorizontalAngle, _settings.MaxHorizontalAngle);
            _angleVer += ver;
            _angleVer = Mathf.Clamp(_angleVer, -_settings.MaxVerticalAngle + 10, _settings.MaxVerticalAngle);

            float angle = 0;
            RaycastHit hit;
            //A��lacakkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
            if (Physics.Raycast(_transform.position, Vector3.down, out hit))
            {
                angle = Vector3.ProjectOnPlane(_transform.position - Vector3.down, hit.normal).y;
            }

            _rotatorH.transform.localRotation = Quaternion.Euler(_rotatorH.transform.localEulerAngles.x, _angleHor, _rotatorH.transform.localEulerAngles.z);
            _rotatorV.transform.localRotation = Quaternion.Euler(-_angleVer, _rotatorV.transform.localEulerAngles.y, _rotatorV.transform.localEulerAngles.z);


            _lastMousePosition = Input.mousePosition;
        }


        [Serializable]
        public class Settings
        {
            public float MaxVerticalAngle = 45;

            public float MaxHorizontalAngle = 60;

            public float HorizontalRotSpeed = 25;

            public float VerticalRotSpeed = 25;

            public float MobileHorizontalRotSpeed = 25;

            public float MobileVerticalRotSpeed = 25;
        }

    }
}
