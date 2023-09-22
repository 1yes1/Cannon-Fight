using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class Ramp : MonoBehaviour
    {


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Cannon"))
            {
                CannonController cannon = other.GetComponent<CannonController>();
                cannon.Boost(GameManager.DefaultRampProperties.BoostMultiplier);
            }
        }

    }
}
