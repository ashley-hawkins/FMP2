using System.Collections;
using System.Collections.Generic;
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
        }

        public List<Recipe> Recipes;

        public virtual void BeginUse(Vector2 position, ItemStack stack = null) { }
    }
}
