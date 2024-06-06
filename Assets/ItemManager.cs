using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FMP
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;
        private ItemBase[] Items;
        private ItemBase.Recipe[] Recipes;

        public ItemBase.Recipe[] GetRecipes()
        {
            if (Recipes == null)
            {
                Recipes = Items.Select(x => x.Recipes).SelectMany(x => x).ToArray();
            }
            return Recipes;
        }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        
        public static ItemBase ItemFromID(ItemID id)
        {
            return instance.Items[(int)id];
        }
    }
}
