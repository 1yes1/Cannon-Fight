using Photon.Pun;
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
    public class AimController : ITickable,IInitializable
    {
        private readonly Settings _settings;

        private readonly Cannon _cannon;

        private readonly CannonView _cannonView;

        private PhotonView _photonView;

        private GameObject _rotatorH;

        private GameObject _rotatorV;

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
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            _cannonView.GetRotators(out _rotatorH, out _rotatorV);
        }

        public void Initialize()
        {
            _photonView = _cannonView.PhotonView;
        }


        public void Tick()
        {
            //return;
            if (!_cannon.CanDoAction)
                return;

            if (!_photonView.IsMine)
                return;

            if (Application.isFocused)
                UnityEngine.Cursor.visible = false;
            else
                return;

#if !UNITY_EDITOR
                SetMobileController();
#else
                SetPcController();
#endif

            //RotateHorizontal();
            //RotateVertical();
        }

        private void SetMobileController()
        {
            if (Input.touchCount == 0)
                return;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);//Input Manager da ayarlanmalý
                bool isRightSide = touch.position.x > Screen.width / 2;

                if (!isRightSide)
                    continue;

                if(touch.phase == TouchPhase.Moved)
                    RotateCannon(touch.deltaPosition,true);
            }
        }

        private void SetPcController()
        {
            RotateCannon(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        }

        private void RotateCannon(Vector3 delta,bool isMobile = false)
        {
            //Debug.Log("Rotate: "+delta.normalized);

            float deltaX = delta.x;
            float deltaY = delta.y;

            float hor = deltaX * ((isMobile) ? _settings.MobileRotationSpeed : _settings.RotationSpeed) * Time.deltaTime;
            //hor = (Mathf.Abs(delta.x) < 0.25f) ? 0 : hor;
            float ver = deltaY * ((isMobile) ? _settings.MobileRotationSpeed : _settings.RotationSpeed) * Time.deltaTime;
            //ver = (Mathf.Abs(delta.y) < 0.25f) ? 0 : ver;

            _angleHor += hor;
            _angleHor = Mathf.Clamp(_angleHor, -_settings.MaxHorizontalAngle, _settings.MaxHorizontalAngle);
            _angleVer += ver;
            _angleVer = Mathf.Clamp(_angleVer, -_settings.MaxVerticalAngle + 10, _settings.MaxVerticalAngle);

            //float angle = 0;
            //RaycastHit hit;
            //Açýlacakkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
            //if (Physics.Raycast(_transform.position, Vector3.down, out hit))
            //{
            //    angle = Vector3.ProjectOnPlane(_transform.position - Vector3.down, hit.normal).y;
            //}

            _rotatorH.transform.localRotation = Quaternion.Euler(_rotatorH.transform.localEulerAngles.x, _angleHor, _rotatorH.transform.localEulerAngles.z);
            _rotatorV.transform.localRotation = Quaternion.Euler(-_angleVer, _rotatorV.transform.localEulerAngles.y, _rotatorV.transform.localEulerAngles.z);
        }

        [Serializable]
        public class Settings
        {
            public float MaxVerticalAngle = 45;

            public float MaxHorizontalAngle = 60;

            public float RotationSpeed = 25;

            public float MobileRotationSpeed = 25;
        }

    }
}
