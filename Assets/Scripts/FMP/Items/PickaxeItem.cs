using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    [CreateAssetMenu(fileName = "NewPickaxeItem", menuName = "FMP/ScriptableObjects/PickaxeItem", order = 1)]
    public class PickaxeItem : ItemBase
    {
        public int miningLevel;
        public override void BeginUse(UseContext ctx, ItemStack stack)
        {
            var wm = WorldManager.instance;
            var gridCoords = wm.WorldToGrid(ctx.position);
            Debug.Log(gridCoords);
            if (wm.BreakBlock(gridCoords, miningLevel)) ++ctx.player.blocksMined;
        }
    }
}
