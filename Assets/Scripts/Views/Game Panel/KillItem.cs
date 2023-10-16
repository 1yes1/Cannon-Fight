using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannonFightBase
{
    public class KillItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _killerText;

        [SerializeField] private TextMeshProUGUI _deadText;

        private Action<KillItem> _onHideAction;

        private RectTransform _rectTransform;

        private CanvasGroup _group;

        private Coroutine _coroutine;

        private float _waitTime;

        private float _hideTime;

        public void Initialize(string killerText,string deadText,float waitTime,float hideTime,Action<KillItem> OnHideAction)
        {
            _group = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _killerText.text = killerText;
            _deadText.text = deadText;
            _waitTime = waitTime;
            _hideTime = hideTime;
            _onHideAction = OnHideAction;

            StartCoroutine(Hide());
        }

        public IEnumerator Hide()
        {
            yield return new WaitForSeconds(_waitTime);

            float count = _hideTime;
            while (count > 0)
            {
                _group.alpha = count / _hideTime;

                count -= Time.deltaTime;

                if (count <= 0)
                {
                    _onHideAction(this);
                    gameObject.SetActive(false);
                }

                yield return null;
            }

        }

        public void MoveToPlace(Vector2 newPosition, float speed)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(IEMoveToPlace(newPosition, speed));
        }

        public IEnumerator IEMoveToPlace(Vector2 newPosition,float speed)
        {
            while (Vector2.Distance(_rectTransform.anchoredPosition,newPosition) > 0.1f)
            {
                _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, newPosition, Time.deltaTime * speed);
                yield return null;
            }

            _coroutine = null;
        }

    }
}
