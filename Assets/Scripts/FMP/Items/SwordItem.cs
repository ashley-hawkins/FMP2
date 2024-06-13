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
            if (ctx.player.swordCooldown)
            {
                return;
            }

            ctx.player.swordCooldown = true;
            // ctx.player.GetComponent<Combat>().DealDamage(10, true); // TEST TODO: REMOVE
            bool isRight = ctx.player.transform.position.x < ctx.position.x;

            var atk = Instantiate(prefab, ctx.player.transform, false).GetComponent<SwordAttack>();
            atk.isRight = isRight;
            atk.sr.sprite = WorldSprite;
            atk.dmg = damage;
        }
    }
}
