using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class Enemy : MonoBehaviour
    {
        private Combat combat;
        private Rigidbody2D rb;
        LayerMask groundLayerMask;

        void OnKnockback(bool right)
        {
            var multiplier = right ? 1 : -1;
            var force = new Vector2(10f * multiplier, 3f) * 15;
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        void Start()
        {
            groundLayerMask = LayerMask.GetMask("Default");
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            combat = GetComponent<Combat>();
            combat.OnDeath += OnDeath;
            combat.OnDamage += OnDamage;
            combat.OnKnockback += OnKnockback;
        }

        bool ShouldJump(float wantsDirection)
        {
            List<RaycastHit2D> hits = new();
            Physics2D.Raycast((Vector2)transform.position - new Vector2(0, 8), new Vector2(wantsDirection, 0).normalized, new ContactFilter2D().NoFilter(), hits, 32);
            Debug.DrawRay((Vector2)transform.position - new Vector2(0, 8), new Vector2(wantsDirection, 0).normalized * 32);
            foreach (var hit in hits)
            {
                var obj = hit.collider.gameObject;
                //if (obj != gameObject && !obj.CompareTag("Player"))
                if (obj.name == "Tilemap")
                {
                    return true;
                }
            }
            return false;
        }

        void Update()
        {
            bool canJump = false;
            {
                //var collider = sr.GetComponent<BoxCollider2D>();
                Vector3 centrePoint = new Vector3(transform.position.x, transform.position.y - 16f);
                //Debug.DrawLine(centrePoint, centrePoint + Vector3.up * 0.1f, Color.black, 1f);
                //print(centrePoint);
                Collider2D overlap = Physics2D.OverlapBox(centrePoint, new Vector2(16 * 1.8f, 0.00001f), 0, groundLayerMask);
                if (overlap != null)
                {
                    canJump = true;
                }
            }
            var dist = playerTransform.position - transform.position;
            if (dist.magnitude > 20 * 32) return;

            var wantsDirection = (Mathf.Abs(dist.x) > 0.1) ? Mathf.Sign(dist.x) : 0;

            //if (dist.y >= 0.9f && Time.time >= nextJump)
            if (ShouldJump(wantsDirection) && Time.time >= nextJump && canJump)
            {
                nextJump = Time.time + 0.8f;
                rb.AddForce(Vector2.up * rb.mass * 16.0f * 7, ForceMode2D.Impulse);
            }

            if (wantsDirection == 1)
            {
                facingRight = true;
            }
            else if (wantsDirection == -1)
            {
                facingRight = false;
            }

            sr.flipX = !facingRight;

            //////////////////////////////////////////////

            float maxSpeed = 8f * 6f;
            float desiredSpeed = maxSpeed * wantsDirection;

            float currentSpeed = rb.velocity.x;

            float desiredSpeedDelta = Mathf.Round(desiredSpeed - currentSpeed);

            float forceRequired = 10f * rb.mass * (desiredSpeedDelta != 0 ? Mathf.Sign(desiredSpeedDelta) : 0);
            if (desiredSpeed == 0) forceRequired /= 2f;
            if (Mathf.Abs(desiredSpeedDelta) < 1 && desiredSpeed == 0)
            {
                forceRequired = 0;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (forceRequired != 0)
            {
                if (desiredSpeed != 0)
                {
                    var desiredDirection = Mathf.Sign(desiredSpeed);
                    //FaceDirection(desiredDirection > 0);
                }
            }

            rb.AddForce(Vector2.right * forceRequired * Time.deltaTime * 250 * 8);

            //animator.SetBool("Walking", desiredSpeed != 0);
        }


        public Transform playerTransform;
        SpriteRenderer sr;

        bool facingRight;

        float nextJump = 0;

        void OnDeath()
        {
            // TODO: Notify death to enemyspawner

            //GameoverScreen.headerText = "You win";
            //UnityEngine.SceneManagement.SceneManager.LoadScene("Gameover");
            Destroy(gameObject);
        }

        void OnDamage(int amount)
        {
            print("ow oof i'm damaged");
        }

        float nextAttack = 0;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (Time.time < nextAttack || !(collision.transform.parent?.CompareTag("Player") ?? false)) return;

            collision.transform.parent.GetComponent<Combat>().DealDamage(10, facingRight);
            nextAttack = Time.time + 1;
        }
    }
}
