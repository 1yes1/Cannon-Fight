using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CannonFightBase
{
    public class JoystickView : UIView
    {
        public enum JoystickLayer
        {
            Drive,
            Aim
        }

        [SerializeField] private GameObject _driveJoystick;

        [SerializeField] private GameObject _aimJoystick;

        private int _driveLayer;

        private int _aimLayer;

        public override void Initialize()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            _driveLayer = LayerMask.NameToLayer("DriveJoystickUI");
            _aimLayer = LayerMask.NameToLayer("AimJoystickUI");

            if (!GameManager.Instance.useAndroidControllers)
                Hide();
        }

        //private void Update()
        //{
        //    if (IsPointerOverUIElement(JoystickLayer.Drive))
        //        print("Drive Layer");
        //    else if(IsPointerOverUIElement(JoystickLayer.Aim))
        //        print("Aim Layer");
        //}


        public bool IsPointerOverUIElement(JoystickLayer joystickLayer)
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults(), joystickLayer);
        }

        //Returns 'true' if we touched or hovering on Unity UI element.
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults,JoystickLayer joystickLayer)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                int layer = 0;
                if (joystickLayer == JoystickLayer.Drive)
                    layer = _driveLayer;
                else
                    layer = _aimLayer;

                if (curRaysastResult.gameObject.layer == layer)
                    return true;
            }
            return false;
        }


        //Gets all event system raycast results of current mouse or touch position.
        private List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }


    }
}
