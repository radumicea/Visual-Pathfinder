using UnityEngine;
using TMPro;

namespace UIHandlerNamespace
{
    public class ButtonCreationHandler : MonoBehaviour
    {
        private const int NumButtons = 6;
        [SerializeField] private TMP_Text[] texts;

        private void Start()
        {
            OptimiseTextSizes();
        }

        public static void SetButtonsPosition(int vertical, int horizontal)
        {
            var posRatio = (vertical <= horizontal) ? -0.6F : 0.5F;
            var dw = 0.15F * horizontal;
            var aMin = (vertical <= horizontal) ? new Vector2(0, 1) : new Vector2(0, 0);
            var aMax = (vertical <= horizontal) ? new Vector2(0, 1) : new Vector2(0, 0);

            var canvasTrans = GameObject.Find("Canvas").GetComponent<RectTransform>();

            var a = canvasTrans.Find("ChooseAlgorithm").GetComponent<RectTransform>();
            a.anchorMin = aMin;
            a.anchorMax = aMax;
            if (a.rect.width * NumButtons > canvasTrans.rect.width)
            {
                dw = 0;
                a.sizeDelta = new Vector2(canvasTrans.rect.width / NumButtons, a.rect.height);
            }
            a.anchoredPosition = new Vector2(0.5F * a.rect.width + dw, posRatio * a.rect.height);

            var s = canvasTrans.Find("SetStart").GetComponent<RectTransform>();
            s.anchorMin = aMin;
            s.anchorMax = aMax;
            if (s.rect.width * NumButtons > canvasTrans.rect.width)
            {
                s.sizeDelta = new Vector2(canvasTrans.rect.width / NumButtons, s.rect.height);
            }
            s.anchoredPosition = new Vector2(a.anchoredPosition.x + 0.5F * (a.rect.width + s.rect.width) + dw, posRatio * s.rect.height);

            var ips = canvasTrans.Find("SetIPs").GetComponent<RectTransform>();
            ips.anchorMin = aMin;
            ips.anchorMax = aMax;
            if (ips.rect.width * NumButtons > canvasTrans.rect.width)
            {
                ips.sizeDelta = new Vector2(canvasTrans.rect.width / NumButtons, ips.rect.height);
            }
            ips.anchoredPosition = new Vector2(s.anchoredPosition.x + 0.5F * (s.rect.width + ips.rect.width) + dw, posRatio * ips.rect.height);

            var dt = canvasTrans.Find("DestroyTiles").GetComponent<RectTransform>();
            dt.anchorMin = aMin;
            dt.anchorMax = aMax;
            if (dt.rect.width * NumButtons > canvasTrans.rect.width)
            {
                dt.sizeDelta = new Vector2(canvasTrans.rect.width / NumButtons, dt.rect.height);
            }
            dt.anchoredPosition = new Vector2(ips.anchoredPosition.x + 0.5F * (ips.rect.width + dt.rect.width) + dw, posRatio * dt.rect.height);

            var ss = canvasTrans.Find("SetSpeed").GetComponent<RectTransform>();
            ss.anchorMin = aMin;
            ss.anchorMax = aMax;
            if (ss.rect.width * NumButtons > canvasTrans.rect.width)
            {
                ss.sizeDelta = new Vector2(canvasTrans.rect.width / NumButtons, ss.rect.height);
            }
            ss.anchoredPosition = new Vector2(dt.anchoredPosition.x + 0.5F * (dt.rect.width + ss.rect.width) + dw, posRatio * ss.rect.height);

            var sst = canvasTrans.Find("SetSpeedText").GetComponent<RectTransform>();
            sst.anchorMin = aMin;
            sst.anchorMax = aMax;
            if (sst.rect.width * NumButtons > canvasTrans.rect.width)
            {
                sst.sizeDelta = new Vector2(canvasTrans.rect.width / NumButtons, sst.rect.height);
            }
            sst.anchoredPosition = new Vector2(dt.anchoredPosition.x + 0.5F * (dt.rect.width + sst.rect.width) + dw, posRatio * sst.rect.height);

            var c = canvasTrans.Find("ChooseClear").GetComponent<RectTransform>();
            c.anchorMin = aMin;
            c.anchorMax = aMax;
            if (c.rect.width * NumButtons > canvasTrans.rect.width)
            {
                c.sizeDelta = new Vector2(canvasTrans.rect.width / NumButtons, c.rect.height);
            }
            c.anchoredPosition = new Vector2(sst.anchoredPosition.x + 0.5F * (sst.rect.width + c.rect.width) + dw, posRatio * c.rect.height);
        }

        public void OptimiseTextSizes()
        {
            int candidateIndex = 0;
            float maxPreferredWidth = 0;

            for (int i = 0; i < texts.Length; i++)
            {
                float preferredWidth = texts[i].preferredWidth;
                if (preferredWidth.CompareTo(maxPreferredWidth) > 0)
                {
                    maxPreferredWidth = preferredWidth;
                    candidateIndex = i;
                }
            }

            texts[candidateIndex].enableAutoSizing = true;
            texts[candidateIndex].ForceMeshUpdate();
            float optimumPointSize = texts[candidateIndex].fontSize;

            texts[candidateIndex].enableAutoSizing = false;

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].fontSize = optimumPointSize;
            }
        }
    }
}