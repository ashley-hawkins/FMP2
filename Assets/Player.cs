using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class Player : MonoBehaviour
    {
        ItemStack[] inventory;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            // Detect left click from player:
            //   Get co-ordinates in the tile grid and call SetBlock on the WorldManager with the block that the player is holding or if the player is holding a pickaxe then call BreakBlock
            if (Input.GetMouseButtonDown(0))
            {
                var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var gridCoords = WorldManager.instance.WorldToGrid(point);
                print(gridCoords);
            }
        }
    }
}
