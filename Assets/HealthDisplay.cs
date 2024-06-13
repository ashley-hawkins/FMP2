using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class HealthDisplay : MonoBehaviour
    {
        GameObject heartPrefab;

        public void SetHealth(int healthNumber)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

        }
    }
}
