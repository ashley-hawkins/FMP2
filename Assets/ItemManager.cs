using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FMP
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;
        public ItemBase[] Items;
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

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
