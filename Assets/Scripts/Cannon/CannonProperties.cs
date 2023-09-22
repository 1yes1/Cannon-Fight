using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class CannonProperties : MonoBehaviour
    {
        [SerializeField] private CannonBall cannonBall;

        [SerializeField] private float _fireFrequency = 0.75f;

        [SerializeField] private float _fireDamage = 10;

        [SerializeField] private float _fireRange = 50;

        [SerializeField] private float _fireBallScale = 0.7f;

        public CannonBall CannonBall
        {
            get
            {
                return cannonBall;
            }
        }

        public float FireFrequency => _fireFrequency;

        public float FireDamage => _fireDamage;

        public float FireRange => _fireRange;

        public float FireBallScale => _fireBallScale;
        

        private void Start()
        {
            SetProperties();
        }

        private void SetProperties()
        {

        }

        public static void GetPropertiesFromArray(float[] array,out float fireFrequency,out float fireRange)
        {
            fireFrequency = array[0];
            fireRange = array[1];
        }
        
        public static float[] CreatePropertiesArray(float fireFrequency, float fireRange)
        {
            return new float[] { fireFrequency, fireRange };
        }

        public void Die()
        {

        }

        public void SetSkillFireDamage()
        {
            _fireDamage *= 2;
        }

        public void ResetSkillFireDamage(Skill skill)
        {
            _fireDamage /= 2;
        }

    }

}
