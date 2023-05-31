using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class HotbarItem : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI itemNumberText;
        public TMPro.TextMeshProUGUI itemAmountText;
        public UnityEngine.UI.Image image;

        public UnityEngine.UI.Image backgroundImage;

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
            itemNumberText.text = text;
        }

        public void UpdateDisplay(ItemStack stack)
        {
            if (stack.itemId == (int)ItemID.None)
            {
                image.enabled = false;
                itemAmountText.text = "";
                return;
            }
            image.enabled = true;
            itemAmountText.text = stack.amount.ToString();
            image.sprite = ItemManager.instance.Items[stack.itemId].Icon;
        }

        public void SetSelected(bool selected)
        {
            backgroundImage.color = selected ? selectedColor : deselectedColor;
        }
    }
}
