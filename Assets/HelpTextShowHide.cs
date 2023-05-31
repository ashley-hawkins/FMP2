using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class HelpTextShowHide : MonoBehaviour
    {
        // Start is called before the first frame update
        bool hidden = false;
        TMPro.TextMeshProUGUI text;
        void Start()
        {
            text = GetComponent<TMPro.TextMeshProUGUI>();
            text.enabled = !hidden;
        }

        // Update is called once per frame
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
