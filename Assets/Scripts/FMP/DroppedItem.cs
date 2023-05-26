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
        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            sr.sprite = itemStack.item.Icon;
        }

        // Update is called once per frame
        void Update()
        { 
            if (follow != null)
            {
                // TODO: gravitate towards player, if touching then player picks up.
            }
        }

        public void RandomizeHorizontalSpeed()
        {
            const float factor = 3f;
            rb.velocity = new Vector2(Random.Range(factor * -16f, factor * 16f), Random.Range(0, factor * 16f));
        }
    }
}
