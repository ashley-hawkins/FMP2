using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace FMP
{
    public abstract class SlotClickedHandler : MonoBehaviour
    {
        public abstract void SlotClicked(int idx);
    }

    public class Hotbar : SlotClickedHandler
    {
        public GameObject hotbarItemPrefab;
        public GameObject craftingButton;
        public CraftingMenu craftingMenu;
        List<HotbarItem> slots;
        const int numHotbarSlots = 10;
        bool isOpen = false;
        ItemStack draggedItem = new()
        {
            itemId = ItemID.None,
            amount = 0
        };
        [SerializeField]
        ItemIcon draggedItemInUi;
        [SerializeField]
        Player player;
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
            SetInventoryDisplay(isOpen);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                isOpen = !isOpen;
                SetInventoryDisplay(isOpen);
            }
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
            if (!shouldDisplay)
                craftingMenu.SetOpened(shouldDisplay);
            foreach (var slot in slots.Skip(numHotbarSlots))
            {
                slot.gameObject.SetActive(shouldDisplay);
            }
            craftingButton.SetActive(shouldDisplay);
        }

        public void BeginDragging(int idx)
        {
            var currentInventorySlotContent = player.inventory[idx];
            if (currentInventorySlotContent == null)
            {
                currentInventorySlotContent = new()
                {
                    itemId = ItemID.None,
                    amount = 0
                };
            }

            player.inventory[idx] = draggedItem;
            draggedItem = currentInventorySlotContent;

            draggedItemInUi.SetItemNumberText("");
            draggedItemInUi.UpdateDisplay(currentInventorySlotContent);

            UpdateDisplay(player.inventory);
        }

        public override void SlotClicked(int idx)
        {
            if (!isOpen) return;

            BeginDragging(idx);
        }
    }
}
