using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public abstract class ItemBase : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public Sprite WorldSprite;

        [System.Serializable]
        public struct Recipe
        {
            public List<ItemStack> recipe;
        }

        public List<Recipe> Recipes;

        public abstract void BeginUse(Vector2 position, ItemStack stack = null);
    }
}
