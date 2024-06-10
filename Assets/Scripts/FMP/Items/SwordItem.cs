using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    [CreateAssetMenu(fileName = "NewSwordItem", menuName = "FMP/ScriptableObjects/SwordItem", order = 1)]
    public class SwordItem : ItemBase
    {
        public int damage;
        public GameObject prefab;
        public override void BeginUse(UseContext ctx, ItemStack stack)
        {
            // ctx.player.GetComponent<Combat>().DealDamage(10, true); // TEST TODO: REMOVE
            var atk = Instantiate(prefab, ctx.player.transform, false).GetComponent<SwordAttack>();
        }
    }
}
