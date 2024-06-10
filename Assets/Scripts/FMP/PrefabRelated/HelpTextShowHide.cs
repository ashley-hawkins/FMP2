using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class HelpTextShowHide : MonoBehaviour
    {
        bool hidden = true;
        TMPro.TextMeshProUGUI text;
        void Start()
        {
            text = GetComponent<TMPro.TextMeshProUGUI>();
            text.enabled = !hidden;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                hidden = !hidden;
                text.enabled = !hidden;
            }
        }
    }
}
