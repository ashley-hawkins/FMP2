using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FMP
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "FMP/ScriptableObjects/ItemBase", order = 0)]
    public class ItemBase : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public Sprite WorldSprite;

        [System.Serializable]
        public struct Recipe
        {
            public List<ItemStack> itemStacks;

            public bool isCraftable(List<ItemStack> availableResources)
            {
                return itemStacks.All(
                    // Which means for the itemStack: there is enough of the item in the available resources
                    stack => availableResources.Any(
                        resource => resource.itemId == stack.itemId && resource.amount >= stack.amount
                    )
                );
            }
        }

        public List<Recipe> Recipes;

        public virtual void BeginUse(Vector2 position, ItemStack stack = null) { }
    }
}
