using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CannonFightBase
{
    public class InputManager: MonoBehaviour
    {

        public static float HorizontalAxis => Input.GetAxis("Mouse X");

        public static float VerticalAxis => Input.GetAxis("Mouse Y");

        public static bool IsFiring => Input.GetMouseButtonDown(0);

        public static bool ShowScoreboard => Input.GetKey(KeyCode.Tab);

    }
}
