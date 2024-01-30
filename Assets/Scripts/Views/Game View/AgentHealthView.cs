using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class AgentHealthView : MonoBehaviour
    {
        [SerializeField] private Image _healthSlider;

        public void SetHealth(int health)
        {
            _healthSlider.fillAmount = health * 0.01f;
        }

    }
}
