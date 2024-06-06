using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    [CreateAssetMenu(fileName = "NewBlockItem", menuName = "FMP/ScriptableObjects/BlockItem", order = 1)]
    public class BlockItem : ItemBase
    {
        public TileType tile;
        public override void BeginUse(Vector2 position, ItemStack stack)
        {
            var wm = WorldManager.instance;
            var gridCoords = wm.WorldToGrid(position);
            Debug.Log(gridCoords);
            var block = new Block() { tileType = tile };
            if (wm.SetBlock(gridCoords, block, false))
            {
                stack.Add(-1);
            }
        }
    }
}
