using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FMP
{
    public class HelpTextShowHide : MonoBehaviour
    {
        bool hidden = true;
        TMPro.TextMeshProUGUI text;

        public string altText;
        string mainText;

        public void DoHideOrUnhide(bool hide)
        {
            if (hide)
            {
                mainText = text.text;
                text.text = altText;
            }
            else
            {
                text.text = mainText;
            }
        }

        void Start()
        {
            text = GetComponent<TMPro.TextMeshProUGUI>();
            DoHideOrUnhide(hidden);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                hidden = !hidden;
                DoHideOrUnhide(hidden);
            }
        }
    }
}
