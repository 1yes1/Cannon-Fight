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

        public CarController.MovementSettings MovementSettings => _MovementSettings;

        public FireState.Settings AimSettings => _aimSettings;

        public Transform CannonBallSpawnTransform => _cannonBallSpawnTransform;

    }
}
