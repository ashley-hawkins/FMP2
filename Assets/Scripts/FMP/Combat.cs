using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class Combat : MonoBehaviour
    {
        public int maxHealth = 100;
        public event System.Action OnDeath;
        public event System.Action<int> OnDamage;
        public event System.Action<bool> OnKnockback;
        public bool alive = true;
        public int currentHealth;
        // Start is called before the first frame update
        void Awake()
        {
            currentHealth = maxHealth;
        }

        public void Heal(int amount)
        {
            if (!alive) return;

            currentHealth += System.Math.Min(amount, System.Math.Max(maxHealth - currentHealth, 0));
        }

        public void DealDamage(int damageAmount, bool right)
        {
            if (!alive) return;

            OnDamage?.Invoke(damageAmount);
            OnKnockback?.Invoke(right);

            currentHealth -= System.Math.Min(damageAmount, System.Math.Max(currentHealth, 0));
            if (currentHealth <= 0)
            {
                alive = false;
                OnDeath?.Invoke();
            }
        }
    }
}