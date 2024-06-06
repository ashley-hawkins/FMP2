using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace FMP
{
    public class CraftingMenu : MonoBehaviour
    {
        GameObject itemSlotPrefab;
        List<ItemBase> craftables;

        [SerializeField]
        Player player;

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
                             recipe => recipe.itemStacks.All(
                                 // Which means for the itemStack: there is enough of the item in the available resources
                                 stack => availableResources.Any(
                                     resource => resource.itemId == stack.itemId && resource.amount >= stack.amount
                                 )
                             )
                         )
                     )
                     .ToList();
        }
        

        void Refresh()
        {
        }

        public void CheckCraftables()
        {
            var craftables = GetCraftableItems(player.GetTotalAmounts());
            foreach (var item in craftables)
            {
                print(item);
            }
        }
    }
}
