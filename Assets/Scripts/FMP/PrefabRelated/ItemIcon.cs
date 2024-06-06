using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class ItemIcon : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI itemNumberText;
        public TMPro.TextMeshProUGUI itemAmountText;
        public UnityEngine.UI.Image image;

        public void SetItemNumberText(string text)
        {
            itemNumberText.text = text;
        }

        public void UpdateDisplay(ItemStack stack)
        {
            if (stack.itemId == ItemID.None)
            {
                image.enabled = false;
                itemAmountText.text = "";
                return;
            }
            image.enabled = true;
            itemAmountText.text = stack.amount.ToString();
            image.sprite = stack.item.Icon;
        }
    }
}
