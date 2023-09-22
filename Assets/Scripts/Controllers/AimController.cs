using CannonFightUI;
using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

namespace CannonFightBase
{
    public class AimController : MonoBehaviour
    {
        [SerializeField] private float _maxVerticalAngle = 45;

        [SerializeField] private float _maxHorizontalAngle = 60;

        [SerializeField] private float _horizontalRotSpeed = 25;

        [SerializeField] private float _verticalRotSpeed = 25;

        [SerializeField] private float _mobileHorizontalRotSpeed = 25;

        [SerializeField] private float _mobileVerticalRotSpeed = 25;

        private PhotonView _photonView;

        private Camera _camera;

        private Cannon _cannon;

        private GameObject _rotatorH;

        private GameObject _rotatorV;

        private float _horizontalAngle = 0;

        private float _verticalAngle = 0;

        private Vector3 _initialForward;

        private Vector3 _initialRight;

        private Vector3 _initialUp;

        private Vector3 _lastMousePosition;

        private float _angleHor;

        private float _angleVer;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();

            if (!_photonView.IsMine)
                return;

            _cannon = GetComponent<Cannon>();

            _camera = Camera.main;

            UnityEngine.Cursor.lockState = CursorLockMode.Confined;

            _cannon.GetRotators(out _rotatorH, out _rotatorV);

        }

        void Start()
        {
            _lastMousePosition = Input.mousePosition;
        }

        void Update()
        {
            if (!_photonView.IsMine)
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
                Touch touch = Input.GetTouch(i);//Input Manager da ayarlanmalý
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
            float hor = delta.x * _horizontalRotSpeed * Time.deltaTime;
            hor = (Mathf.Abs(delta.x) < 0.25f) ? 0 : hor;
            float ver = delta.y * _verticalRotSpeed * Time.deltaTime;
            ver = (Mathf.Abs(delta.y) < 0.25f) ? 0 : ver;

            _angleHor += hor;
            _angleHor = Mathf.Clamp(_angleHor, -_maxHorizontalAngle, _maxHorizontalAngle);
            _angleVer += ver;
            _angleVer = Mathf.Clamp(_angleVer, -_maxVerticalAngle, _maxVerticalAngle);

            float angle = 0;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                angle = Vector3.ProjectOnPlane(transform.position - Vector3.down, hit.normal).y;
            }

            _rotatorH.transform.localRotation = Quaternion.Euler(_rotatorH.transform.localEulerAngles.x, _angleHor, _rotatorH.transform.localEulerAngles.z);
            _rotatorV.transform.localRotation = Quaternion.Euler(-_angleVer, _rotatorV.transform.localEulerAngles.y, _rotatorV.transform.localEulerAngles.z);



            _lastMousePosition = Input.mousePosition;
        }


        public static float ClampAngle(float angle, float min, float max)
        {
            if (min < 0 && max > 0 && (angle > max || angle < min))
            {
                angle -= 360;
                if (angle > max || angle < min)
                {
                    if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                    else return max;
                }
            }
            else if (min > 0 && (angle > max || angle < min))
            {
                angle += 360;
                if (angle > max || angle < min)
                {
                    if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                    else return max;
                }
            }

            if (angle < min) return min;
            else if (angle > max) return max;
            else return angle;
        }

    }

}
