using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class HotbarItem : MonoBehaviour
    {
        private UnityEngine.UI.Image backgroundImage;
        public ItemIcon itemIcon;

        public Color selectedColor;
        public Color deselectedColor;
        void Awake()
        {
            backgroundImage = GetComponent<UnityEngine.UI.Image>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetItemNumberText(string text)
        {
            itemIcon.SetItemNumberText(text);
        }

        public void UpdateDisplay(ItemStack stack)
        {
            itemIcon.UpdateDisplay(stack);
        }

        public void SetSelected(bool selected)
        {
            backgroundImage.color = selected ? selectedColor : deselectedColor;
        }
    }
}
