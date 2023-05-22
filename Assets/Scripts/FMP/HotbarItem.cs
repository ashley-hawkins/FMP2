using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class HotbarItem : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI itemNumberText;
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetItemNumberText(string text)
        {
            itemNumberText.text = text;
        }
    }
}
