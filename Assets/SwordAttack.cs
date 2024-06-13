using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class SwordAttack : MonoBehaviour
    {
        float startTime;
        public bool isRight;
        public SpriteRenderer sr;
        public int dmg;

        private void Start()
        {
            startTime = Time.realtimeSinceStartup;
            sr.flipX = !isRight;
            if (!isRight)
            {
                sr.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
        }
        private void LateUpdate()
        {
            var timeTaken = Time.realtimeSinceStartup - startTime;
            //if (timeTaken > 0.5f)
            //{
            //    Destroy(gameObject);
            //}

            float angle;
            if (isRight)
            {
                angle = 45.0f - timeTaken * 180.0f * 3.0f;
            }
            else
            {
                angle = 45.0f + timeTaken * 180.0f * 3.0f;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
            if (angle < -60.0f || angle > 150.0f)
            {
                transform.parent.GetComponent<Player>().swordCooldown = false;
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            print("Collision entered");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
                if (collision.gameObject.TryGetComponent<Combat>(out var combat))
                {
                    combat.DealDamage(dmg, isRight);
                }
        }
    }
}
