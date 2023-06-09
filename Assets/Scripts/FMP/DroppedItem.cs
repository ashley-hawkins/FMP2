using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class DroppedItem : MonoBehaviour
    {
        public ItemStack itemStack;
        SpriteRenderer sr;
        Rigidbody2D rb;
        public Transform follow;
        // Start is called before the first frame update

        int normalLayer;
        int flyingLayer;
        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            sr.sprite = itemStack.item.Icon;
            follow = Player.instance.transform;

            normalLayer = LayerMask.NameToLayer("Item");
            flyingLayer = LayerMask.NameToLayer("FlyingItem");
        }

        // Update is called once per frame
        void FixedUpdate()
        { 
            if (follow != null)
            {
                var difference = follow.transform.position - sr.transform.position;
                if (difference.magnitude < 16 * 4)
                {
                    // TODO: gravitate towards player, if touching then player picks up.
                    rb.AddForce(difference.normalized * 800f);
                    // https://stackoverflow.com/a/60587061
                    // If the item is traveling towards the player, and is are within the player's pickup range (implied by enclosing if statement)
                    // then disable collisions with terrain, by using the flying item layer instead of the normal one.
                    if (Vector3.Dot(rb.velocity, difference.normalized) >= 0)
                    {
                        gameObject.layer = flyingLayer;
                        return;
                    }
                }
            }
            gameObject.layer = normalLayer;
        }

        public void RandomizeHorizontalSpeed()
        {
            const float factor = 3f;
            rb.velocity = new Vector2(Random.Range(factor * -16f, factor * 16f), Random.Range(0, factor * 16f));
        }
    }
}
