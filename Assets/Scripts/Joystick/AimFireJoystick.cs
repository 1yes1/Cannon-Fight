using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class AimFireJoystick : CFBehaviour
    {
        [SerializeField] private Button _fireButton;

        public override void RegisterCallerEvents()
        {
            _fireButton.onClick.AddListener(GameEventCaller.Instance.OnMobileFireButtonClicked);
        }

    }
}
