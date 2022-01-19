using System.Collections;
using TMPro;
using UnityEngine;

namespace UIHandlerNamespace
{
    public class UnsetErrorDisplayer : MonoBehaviour
    {
        private TMP_Text uemText;

        private void Start()
        {
            var canvasTrans = GameObject.Find("Canvas").GetComponent<RectTransform>();
            var uem = canvasTrans.Find("UnsetErrorMessage");
            uem.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasTrans.rect.width / 2, canvasTrans.rect.height / 2);
            uemText = uem.GetComponent<TMP_Text>();
            uemText.alpha = 0;
        }

        public void Display()
        {
            StartCoroutine(DisplayHelper());
        }

        private IEnumerator DisplayHelper()
        {
            uemText.alpha = 1;
            yield return new WaitForSeconds(4);
            uemText.alpha = 0;
        }
    }
}