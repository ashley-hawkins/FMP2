using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FMP
{
    public class Hotbar : MonoBehaviour
    {
        public GameObject hotbarItemPrefab;
        List<HotbarItem> slots;
        const int numHotbarSlots = 10;
        // Start is called before the first frame update
        void Start()
        {
            slots = new List<HotbarItem>();
            for (int i = 0; i < Player.InventorySize; ++i)
            {
                var item = Instantiate(hotbarItemPrefab, transform).GetComponent<HotbarItem>();
                item.SetItemNumberText($"{i + 1}");
                slots.Add(item);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void UpdateDisplay(List<ItemStack> items)
        {
            print("Items count: " + items.Count);
            print("Slots count: " + slots.Count);
            print("updating hotbar");
            foreach (var (slot, item) in slots.Zip(items, (x, y) => (x, y)))
            {
                print("Updating individual slot");
                slot.UpdateDisplay(item);
            }
        }
        public void DisplaySelectedIndex(int selectIndex)
        {
            int currentIndex = 0;
            foreach (var slot in slots)
            {
                bool isSelected = currentIndex == selectIndex;
                slot.SetSelected(isSelected);
                currentIndex++;
            }
        }
        public void SetInventoryDisplay(bool shouldDisplay)
        {
            foreach (var slot in slots.Skip(numHotbarSlots))
            {
                slot.gameObject.SetActive(shouldDisplay);
            }
        }
    }
}
