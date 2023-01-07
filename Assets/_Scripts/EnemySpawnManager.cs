using _Scripts.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public float startTimeBtwSpawn;
        private float timeBtwSpawn;

        public GameObject[] enemies;

        private void Update()
        {

            if (timeBtwSpawn <= 0)
            {
                int randEnemy = Random.Range(0, enemies.Length);
                Instantiate(enemies[randEnemy], MapManager.GetRandomPositionOnMap(), Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            else
            {
                timeBtwSpawn -= Time.deltaTime;
            }
        }
    }
}