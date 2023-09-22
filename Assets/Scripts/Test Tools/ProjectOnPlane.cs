using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class ProjectOnPlane : MonoBehaviour
    {

        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, transform.forward * 20,Color.green);

            RaycastHit hit;
            if(Physics.Raycast(transform.position,transform.forward,out hit))
            {
                Debug.DrawRay(hit.point, hit.normal * 5, Color.blue);
                Debug.DrawRay(hit.point, Vector3.ProjectOnPlane(transform.forward, hit.normal) * 5, Color.red);
                print(Vector3.Angle(hit.point, transform.forward));
            }
        }


    }
}
