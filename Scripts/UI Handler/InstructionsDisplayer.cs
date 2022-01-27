using System.Collections;
using TMPro;
using UnityEngine;

namespace UIHandlerNamespace
{
    public class InstructionsDisplayer : MonoBehaviour
    {
        private TMP_Text instructText;

        private void Start()
        {
            var canvasTrans = GameObject.Find("Canvas").GetComponent<RectTransform>();
            var instruct = canvasTrans.Find("Instructions");
            instruct.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasTrans.rect.width / 1.25F, canvasTrans.rect.height / 3);
            instructText = instruct.GetComponent<TMP_Text>();
            instructText.alpha = 1;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                instructText.alpha = 0;
            }
        }
    }
}