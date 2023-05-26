using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    [CreateAssetMenu(fileName = "NewBlockItem", menuName = "FMP/ScriptableObjects/BlockItem", order = 1)]
    public class BlockItem : ItemBase
    {
        public TileType tile;
        public override void BeginUse(Vector2 position)
        {
            var wm = WorldManager.instance;
            var gridCoords = wm.WorldToGrid(position);
            Debug.Log(gridCoords);
            var block = new Block() { tileType = TileType.Grass };
            wm.SetBlock(gridCoords, block);
        }
    }
}
