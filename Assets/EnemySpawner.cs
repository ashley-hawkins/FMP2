using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FMP
{
    public class EnemySpawner : MonoBehaviour
    {
        public Transform playerTransform;
        float nextSpawnTime;

        public GameObject enemyPrefab;

        public HashSet<GameObject> spawnedEnemies = new();

        public int killedEnemies = 29;

        public float startTime;

        public TMPro.TextMeshProUGUI tmpro;

        private void Start()
        {
            nextSpawnTime = Time.time + 10;
            startTime = Time.time;
        }

        void Update()
        {
            if (Time.time < nextSpawnTime || spawnedEnemies.Count >= 10)
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
                enemy.OnKilled += EnemyDespawned;
                spawnedEnemies.Add(enemy.gameObject);
                nextSpawnTime = Time.time + 10;
            }
        }

        public void EnemyDespawned(GameObject obj, bool died)
        {
            if (died)
            {
                killedEnemies += 1;
            }
            spawnedEnemies.Remove(obj);
            obj.GetComponent<Enemy>().OnKilled -= EnemyDespawned;

            tmpro.text = $"Enemies Killed: {killedEnemies}/10";

            if (killedEnemies == 10)
            {
                EndGame(true);
            }
        }

        public void EndGame(bool won)
        {
            var player = playerTransform.GetComponent<Player>();
            var blocksMined = player.blocksMined;
            var blocksPlaced = player.blocksPlaced;

            GameoverScreen.won = won;
            GameoverScreen.blocksMined = blocksMined;
            GameoverScreen.blocksPlaced = blocksPlaced;
            GameoverScreen.itemsCrafted = player.itemsCrafted;
            GameoverScreen.enemiesKilled = killedEnemies;
            GameoverScreen.timeSurvivedOrTaken = Time.time - startTime;

            SceneManager.LoadScene("Gameover");
        }
    }
}
