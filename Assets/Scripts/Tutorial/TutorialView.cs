using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightUI
{
    public class TutorialView : UIView
    {
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private Image _completedImage;

        private Coroutine _coroutine;

        public override void Initialize()
        {
        }
        public override void AddSubViews()
        {
        }

        public void SetText(string text)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ShowText(text));
        }

        private IEnumerator ShowText(string text)
        {
            _text.text = "";
            _completedImage.fillAmount = 0;

            int index = 0;
            while (text.Length > index)
            {
                _text.text += text[index];
                index++;
                yield return new WaitForSeconds(0.02f);
            }
        }

        public void SetCompleted()
        {
            StartCoroutine(ShowCompletedImage());
        }

        private IEnumerator ShowCompletedImage()
        {
            _completedImage.fillOrigin = 0;

            float count = 0;
            float time = 0.2f;
            _completedImage.fillAmount = 0;
            while (count <= time)
            {
                count+=Time.deltaTime;
                _completedImage.fillAmount = count / time;
                yield return null;
            }
            _completedImage.fillAmount = 1;

            StartCoroutine(BackToNormalImage());
        }

        private IEnumerator BackToNormalImage()
        {
            yield return new WaitForSeconds(1);
            
            _completedImage.fillOrigin = 1;

            float count = 0;
            float time = 0.2f;
            _completedImage.fillAmount = 1;
            while (count <= time)
            {
                count += Time.deltaTime;
                _completedImage.fillAmount = 1 - (count / time);
                yield return null;
            }
            _completedImage.fillAmount = 0;
        }
    }
}
