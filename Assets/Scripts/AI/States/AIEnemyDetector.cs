using UnityEngine;

namespace CannonFightBase
{
    public class AIEnemyDetector
    {
        private Agent.ViewSettings _viewSettings;

        private readonly FireState.Settings _settings;

        private AgentView _agentView;

        private Transform _targetEnemy;

        public AIEnemyDetector(AgentView agentView,Agent.ViewSettings viewSettings)
        {
            _viewSettings = viewSettings;
            _agentView = agentView;
            _settings = _agentView.AimSettings;
        }

        public Transform FindClosestEnemy()
        {
            Collider[] hitColliders = Physics.OverlapSphere(_agentView.transform.position, _viewSettings.ViewRadius, _viewSettings.EnemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<AgentView>() != _agentView)
                {
                    _targetEnemy = hitCollider.transform;
                    return _targetEnemy;
                }
            }

            //AgentView[] agentViews = GameObject.FindObjectsOfType<AgentView>();

            //for (int i = 0; i < agentViews.Length; i++)
            //{
            //    if (agentViews[i] != _agentView)
            //        return agentViews[i].transform;
            //}

            return null;
        }

        public bool HasFireAngle()
        {
            if (_targetEnemy == null)
                return false;

            Vector3 dir = _targetEnemy.position - _agentView.transform.position;

            float dot = Vector3.Dot(_agentView.transform.forward, dir.normalized);

            if (dot >= 0.85f)
            {
                //Debug.Log("Atýþ Açýsý Var: "+ dot);
                return true;

            }
            else
            {
                //Debug.Log("Atýþ Açýsý YOk: "+ dot);
                return false;

            }
            //NoViewAngle();
        }

    }
}
