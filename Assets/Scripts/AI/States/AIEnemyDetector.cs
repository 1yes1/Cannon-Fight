using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AIEnemyDetector
    {
        private Agent.ViewSettings _viewSettings;

        private AgentView _agentView;

        private Transform _targetEnemy;

        public AIEnemyDetector(AgentView agentView,Agent.ViewSettings viewSettings)
        {
            _viewSettings = viewSettings;
            _agentView = agentView;
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

    }
}
