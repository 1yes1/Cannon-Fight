using CannonFightBase;
using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private PhotonView _photonView;

    private Cannon cannon;

    private Vector3 _defaultTrackedObjectOffset;

    private void OnEnable()
    {
        GameEventReceiver.OnBoostStartedEvent += OnBoostStarted;
        GameEventReceiver.OnBoostEndedEvent += OnBoostEnded;
    }

    private void OnDisable()
    {
        GameEventReceiver.OnBoostStartedEvent -= OnBoostStarted;
        GameEventReceiver.OnBoostEndedEvent -= OnBoostEnded;
    }



    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cannon = GetComponentInParent<Cannon>();
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //print(transform.parent.name);

        if (_photonView.IsMine)
            SetVirtualCamera();
    }

    private void SetVirtualCamera()
    {
        virtualCamera.Priority = 999;
        //GameObject rotatorH;
        //GameObject rotatorV;
        //cannon.GetRotators(out rotatorH, out rotatorV);

        //virtualCamera.LookAt = AimController.CrosshairPoint.transform;
        //virtualCamera.Follow = rotatorV.transform;
    }

    private void OnBoostStarted(Cannon cannon)
    {
        if (!_photonView.IsMine)
            return;

        _defaultTrackedObjectOffset = virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;

        SetRampSettings();
    }

    private void OnBoostEnded(Cannon cannon)
    {
        if (!_photonView.IsMine)
            return;

        GetComponent<Cinemachine3rdPersonAim>().enabled = true;
        virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = _defaultTrackedObjectOffset;
    }

    public void SetRampSettings()
    {
        virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = 0;
        //GetComponent<Cinemachine3rdPersonAim>().enabled = false;
    }

}
