using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Zenject;

namespace CannonFightBase
{
    public class Potion : MonoBehaviour, IPoolable<IMemoryPool>,IDisposable
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private ParticleSettings _particleSettings;

        protected Skills _skill;
        
        private IMemoryPool _pool;

        private Transform _target;

        private Animation _animation;

        private float _totalFlightTime = 1;

        private bool _isShowedUp = false;

        public Skills Skill => _skill;

        [Inject]
        public void Construct(ParticleSettings particleSettings)
        {
            _particleSettings = particleSettings;
            _particleSettings.Initialize();
        }

        private void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        public void ShowUp(float totalFlightTime, Transform target)
        {
            if (_isShowedUp)
                return;

            _totalFlightTime = totalFlightTime;
            _target = target;

            _isShowedUp = true;

            _animation.Play();

            transform.parent = null;

            StartCoroutine(StartShowUpAnimation());
        }

        //public abstract void SetSkill();

        public void Initialize(PotionType potionType)
        {
            _meshRenderer.sharedMaterial = potionType.Material;
            _skill = potionType.Skill;
        }

        //private void OnDrawGizmos()
        //{
        //    if (_lineRenderer == null)
        //        return;

        //    //Debug.DrawRay(transform.position, Vector3.down * 100, Color.red);
        //    Vector3 targetPos = _target.transform.position;
        //    Vector3 startPos = transform.position;
        //}

        private IEnumerator StartShowUpAnimation()
        {
            Vector3 targetPos = _target.transform.position;
            Vector3 startPos = transform.position;

            float distX = targetPos.x - startPos.x;
            float distY = targetPos.y - startPos.y;
            float distZ = targetPos.z - startPos.z;

            float gravity = -Physics.gravity.y;

            float horizontalSpeed = distX / _totalFlightTime;
            float verticalSpeed = (distY + 0.5f * gravity * _totalFlightTime * _totalFlightTime) / _totalFlightTime;
            float zSpeed = distZ / _totalFlightTime;

            float t = 0;

            while (Vector3.Distance(targetPos, transform.position) > 0.1f)
            {
                float x = startPos.x + horizontalSpeed * t;
                float y = startPos.y + verticalSpeed * t - 0.5f * gravity * t * t;
                float z = startPos.z + zSpeed * t;

                Vector3 currentPosition = new Vector3(x, y, z);
                transform.position = currentPosition;
                t += Time.deltaTime;

                yield return null;
            }

        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Cannon"))
            {
                if(other.GetComponent<Cannon>().CannonSkillHandler.CanCollectPotion(_skill))
                {
                    other.GetComponent<Cannon>().OnPotionCollected(this);
                    ParticleManager.CreateAndPlay(ParticleManager.GetParticleTupleValue(_particleSettings.ParticleTuplesByType, _skill),null,transform.position);
                    //Destroy(gameObject);
                    Dispose();
                }
            }
        }

        public void OnDespawned()
        {
            _isShowedUp = false;
        }

        public void OnSpawned(IMemoryPool p1)
        {
            _pool = p1;
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Potion>
        {

        }


        public class Pool : MonoPoolableMemoryPool<IMemoryPool, Potion>
        {

        }

        [Serializable]
        public class Settings
        {
            public List<PotionType> PotionTypes;
        }

        [Serializable]
        public class PotionType
        {
            public Skills Skill;
            public Material Material;
        }

        [Serializable]
        public class ParticleSettings
        {
            public List<ParticleTuple<Skills>> SkillsParticleTuples;

            public Dictionary<Type, IList> ParticleTuplesByType;

            public void Initialize()
            {
                ParticleTuplesByType = new Dictionary<System.Type, IList>
                {
                    { typeof(Skills), SkillsParticleTuples },
                };
            }
        }

    }


}