using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class AimFireJoystick : UISubView, IEventSubscriber
    {
        [SerializeField] private Button _fireButton;

        public override void Initialize()
        {
        }

        public override void SetParameters(params object[] objects)
        {
        }

        public void SubscribeEvent()
        {
            _fireButton.onClick.AddListener(GameEventCaller.Instance.OnMobileFireButtonClicked);
        }
    }
}
