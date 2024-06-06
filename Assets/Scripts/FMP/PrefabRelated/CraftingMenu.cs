using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static FMP.ItemBase;

namespace FMP
{
    public class CraftingMenu : SlotClickedHandler
    {
        [SerializeField]
        GameObject itemSlotPrefab;
        List<ItemID> craftables;

        [SerializeField]
        Player player;

        bool isOpen = false;

        List<ItemID> GetCraftableItems(List<ItemStack> availableResources)
        {
            var allItemIds = Enumerable
                               .Range((int)ItemID.FirstItem,
                                      (int)ItemID.LastItem - (int)ItemID.FirstItem + 1)
                               .Select(id => (ItemID)id);

            return allItemIds
                     .Where(
                         // Which means: any of the recipes is valid
                         itemId => ItemManager.ItemFromID(itemId).Recipes.Any(
                             // Which means for the recipe: all of the itemStacks are satisfied
                             recipe => recipe.isCraftable(availableResources)
                         )
                     )
                     .ToList();
        }

        private void Start()
        {
            SetOpened(isOpen);
        }

        bool TryCraft(int idx)
        {
            var itemId = craftables[idx];
            foreach (var recipe in ItemManager.ItemFromID(itemId).Recipes)
            {
                if (!recipe.isCraftable(player.GetTotalAmounts())) continue;
                foreach (var itemStack in recipe.itemStacks)
                {
                    var remaining = itemStack.amount;
                    foreach (var slot in player.inventory.FindAll(x => x.itemId == itemStack.itemId))
                    {
                        if (remaining <= 0)
                        {
                            break;
                        }
                        var maxRemovedAmount = System.Math.Min(slot.amount, remaining);
                        remaining -= maxRemovedAmount;
                        slot.Add(-maxRemovedAmount);
                    }

                    if (remaining > 0)
                    {
                        throw new System.NotImplementedException("Unrecoverable problem occurred with crafting system, craft failed to find required resources after isCraftable succeeded");
                    }
                }
                player.AddItemStackToInventory(new ItemStack()
                {
                    itemId = itemId,
                    amount = 1
                });
                return true;
            }
            return false;
        }

        void DoCraft(int idx)
        {
            if (TryCraft(idx))
            {
                player.hotbar.UpdateDisplay(player.inventory);
            }
            Refresh();
        }

        public void Refresh()
        {
            craftables = GetCraftableItems(player.GetTotalAmounts());

            for (int i = 0; i < transform.childCount; ++i)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            if (craftables.Count == 0)
            {
                // Placeholder empty gameobject
                var obj = new GameObject().AddComponent<RectTransform>();
                obj.transform.SetParent(transform, false);
            }
            foreach (var item in craftables)
            {
                {
                    var x = Instantiate(itemSlotPrefab, transform);
                    HotbarItem hotbarItem = x.GetComponent<HotbarItem>();
                    hotbarItem.UpdateDisplay(new ItemStack()
                    {
                        itemId = item,
                        amount = 1
                    });
                }
            }
        }

        public void CheckCraftables()
        {
            var craftables = GetCraftableItems(player.GetTotalAmounts());
            foreach (var item in craftables)
            {
                print(item);
            }
        }

        public override void SlotClicked(int idx)
        {
            DoCraft(idx);
        }

        public void SetOpened(bool p_isOpen)
        {
            isOpen = p_isOpen;
            if (isOpen)
                Refresh();
            gameObject.SetActive(isOpen);
        }

        public void ToggleOpened()
        {
            SetOpened(!isOpen);
        }
    }
}
