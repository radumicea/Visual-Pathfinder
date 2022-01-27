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
            uem.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasTrans.rect.width / 1.25F, canvasTrans.rect.height / 3);
            uemText = uem.GetComponent<TMP_Text>();
            uemText.alpha = 0;
        }

        public void Display()
        {
            uemText.alpha = 1;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                uemText.alpha = 0;
            }
        }
    }
}