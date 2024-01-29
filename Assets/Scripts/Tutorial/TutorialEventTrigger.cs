using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CannonFightBase
{
    public class TutorialEventTrigger : MonoBehaviour
    {
        public TutorialEventType tutorialEventType;

        [SerializeField] private UnityEvent OnTriggerEnterEvent;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Cannon"))
            {
                OnTriggerEnterEvent.Invoke();
            }
        }
    }
}
