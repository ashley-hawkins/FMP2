using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class Hotbar : MonoBehaviour
    {
        public int slots;
        public GameObject hotbarItemPrefab;
        // Start is called before the first frame update
        List<HotbarItem> items;
        void Start()
        {
            items = new List<HotbarItem>();
            for (int i = 0; i < slots; ++i)
            {
                var item = Instantiate(hotbarItemPrefab, transform).GetComponent<HotbarItem>();
                item.SetItemNumberText($"{i + 1}");
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
