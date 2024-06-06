using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FMP
{
    public class HotbarItem : MonoBehaviour, IPointerClickHandler
    {
        private UnityEngine.UI.Image backgroundImage;
        public ItemIcon itemIcon;

        public Color selectedColor;
        public Color deselectedColor;

        public SlotClickedHandler hb;

        void Awake()
        {
            backgroundImage = GetComponent<UnityEngine.UI.Image>();
        }

        private void Start()
        {
            hb = GetComponentInParent<SlotClickedHandler>();
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

        public void SelectSelf()
        {
            hb.SlotClicked(transform.GetSiblingIndex());
        }

        public void OnPointerClick(PointerEventData eventData) { SelectSelf(); }
    }
}
