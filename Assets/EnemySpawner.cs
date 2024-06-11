using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FMP
{
    public class EnemySpawner : MonoBehaviour
    {
        public Transform playerTransform;
        float nextSpawnTime;

        public GameObject enemyPrefab;

        public HashSet<GameObject> spawnedEnemies;

        void Update()
        {
            if (Time.time < nextSpawnTime)
            {
                return;
            }

            var pos_offset = new Vector2Int(Random.Range(-50, 50), Random.Range(-50, 50)) * 32 + new Vector2Int(16, 16);
            var pos = (Vector2)playerTransform.position + pos_offset;
            var colliders = Physics2D.OverlapBoxAll(pos, new Vector2(16, 16), 0);
            if (colliders.All(collider => collider.name != "Tilemap"))
            {
                var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity).GetComponent<Enemy>();
                enemy.playerTransform = playerTransform;
                nextSpawnTime = Time.time + 30;
            }
        }

        public void EnemyDespawned(GameObject obj)
        {
            spawnedEnemies.Remove(obj);
        }
    }
}
