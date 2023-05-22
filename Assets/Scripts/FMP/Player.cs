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
            TeleportToSpawn();
        }

        void TeleportToSpawn()
        {
            print("Teleporting to spawn");
            Vector3 newPos = (Vector3Int)WorldManager.instance.worldInfo.spawnPoint * 16;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }

        // Update is called once per frame
        void Update()
        {
            // Detect left click from player:
            //   Get co-ordinates in the tile grid and call SetBlock on the WorldManager with the block that the player is holding or if the player is holding a pickaxe then call BreakBlock
            if (Input.GetMouseButtonDown(0))
            {
                var wm = WorldManager.instance;
                var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var gridCoords = wm.WorldToGrid(point);
                print(gridCoords);
                var block = new Block() { tileType = TileType.Grass };
                wm.SetBlock(gridCoords, block);
            }
            if (Input.GetMouseButtonDown(1))
            {
                var wm = WorldManager.instance;
                var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var gridCoords = wm.WorldToGrid(point);
                print(gridCoords);
                wm.BreakBlock(gridCoords);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                TeleportToSpawn();
            }
        }
    }
}
