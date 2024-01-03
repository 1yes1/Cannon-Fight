using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AgentView : MonoBehaviour
    {
        [SerializeField] private Transform _cannonBallSpawnTransform;

        [SerializeField] private CarController.MovementSettings _MovementSettings;

        [SerializeField] private FireState.Settings _aimSettings;

        private Canvas _canvas;

        private CinemachineVirtualCamera _camera;

        [Inject]
        private void Construct(Canvas canvas,CinemachineVirtualCamera cinemachineVirtualCamera)
        {
            _canvas = canvas;
            _camera = cinemachineVirtualCamera;
        }

        public CarController.MovementSettings MovementSettings => _MovementSettings;

        public FireState.Settings AimSettings => _aimSettings;

        public Transform CannonBallSpawnTransform => _cannonBallSpawnTransform;

        private void Update()
        {
            _canvas.transform.forward = _camera.transform.position - _canvas.transform.position;
        }

    }
}
