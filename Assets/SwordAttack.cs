using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class SwordAttack : MonoBehaviour
    {
        float startTime;
        private void Start()
        {
            startTime = Time.realtimeSinceStartup;
        }
        private void LateUpdate()
        {
            var timeTaken = Time.realtimeSinceStartup - startTime;
            //if (timeTaken > 0.5f)
            //{
            //    Destroy(gameObject);
            //}
            var angle = 45.0f - timeTaken * 180.0f * 2;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            if (angle < -60.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
