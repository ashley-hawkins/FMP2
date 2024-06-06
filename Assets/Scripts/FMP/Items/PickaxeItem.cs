using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    [CreateAssetMenu(fileName = "NewPickaxeItem", menuName = "FMP/ScriptableObjects/PickaxeItem", order = 1)]
    public class PickaxeItem : ItemBase
    {
        public int miningLevel;
        public override void BeginUse(Vector2 position, ItemStack stack)
        {
            var wm = WorldManager.instance;
            var gridCoords = wm.WorldToGrid(position);
            Debug.Log(gridCoords);
            wm.BreakBlock(gridCoords, miningLevel);
        }
    }
}
