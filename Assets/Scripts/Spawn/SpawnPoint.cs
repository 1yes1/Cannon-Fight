using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class SpawnPoint : MonoBehaviour
    {
        public bool _isUsing = false;

        public bool IsUsing
        {
            get { return _isUsing; }
            set { _isUsing = value; }
        }

        public Vector3 Position => transform.position;

        public Quaternion Rotation => transform.rotation;

        private void Start()
        {
            gameObject.SetActive(false);
        }

    }
}
