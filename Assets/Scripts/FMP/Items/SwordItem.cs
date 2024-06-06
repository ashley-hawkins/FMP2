using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    [CreateAssetMenu(fileName = "NewSwordItem", menuName = "FMP/ScriptableObjects/SwordItem", order = 1)]
    public class SwordItem : ItemBase
    {
        public int damage;
        public override void BeginUse(UseContext ctx, ItemStack stack)
        {
            // TODO: this
        }
    }
}
