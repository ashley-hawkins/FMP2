using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class CraftingMenu : MonoBehaviour
    {
        List<ItemBase> craftables;

        List<ItemID> GetCraftableItems(List<ItemStack> availableResources)
        {
            List<ItemID> craftableItems = new();
            List<ItemID> allItems = new(); // placeholder
            foreach (ItemID itemId in allItems)
            {
                var item = ItemManager.instance.Items[(int)itemId];
                foreach (var recipe in item.Recipes)
                {
                    var craftable = true;
                    foreach (var stack in recipe.recipe)
                    {
                        bool resourceFound = false;
                        var stackItemId = stack.itemId;
                        var stackAmount = stack.amount;

                        foreach (var resource in availableResources)
                        {
                            if (resource.itemId == stackItemId && resource.amount <= stackAmount)
                            {
                                resourceFound = true;
                                break;
                            }
                        }
                        if (!resourceFound)
                        {
                            craftable = false;
                            break;
                        }
                    }
                    if (craftable)
                    {
                        craftableItems.Add(itemId);
                    }
                }
            }
            return craftableItems;
        }

        void Refresh()
        {

        }
    }
}
