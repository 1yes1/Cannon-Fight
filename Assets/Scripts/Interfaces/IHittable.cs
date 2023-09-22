using UnityEngine;

namespace CannonFightBase
{
    public interface IHittable
    {
        public void OnHit(Vector3 hitPoint);
    }
}
