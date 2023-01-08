using UnityEngine;

namespace _Scripts
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private Waves waves;

        public int waveNumber;
        public int currentWave;
        private int _currentWaveCount;
        private float waveTimer = 0.0f;
        private float spawnTimer = 0.0f;
        public int maxEnemies = 20;
        private int enemyCount = 0; // Current number of enemies

        public void SetWave(Waves wave)
        {
            waves = wave;
            currentWave = 0;
            waveNumber = 0;
        }

        public void Setup()
        {
            _currentWaveCount = waves.waveList[currentWave].enemyCount;
            spawnTimer = waves.waveList[currentWave].timeBetweenSpawns;
        }

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
        }

        private void SpawnCurrentWave()
        {
            
        }

        private void SpawnWaves()
        {
            waveTimer += Time.deltaTime; // Increment the wave timer

            // Check if it's time to start a new wave
            if (waveTimer >= waves.timeBetweenWaves && enemyCount < maxEnemies)
            {
                // Reset the wave timer
                waveTimer = 0.0f;

                // Get the current wave data
                WaveData currentWaveData = waves.waveList[currentWave];

                // Set the spawn timer to the time between spawns for the current wave
                spawnTimer = currentWaveData.timeBetweenSpawns;
            }

            // Check if it's time to spawn an enemy
            if (spawnTimer >= waves.timeBetweenWaves && enemyCount < maxEnemies)
            {
                // Reset the spawn timer
                spawnTimer = 0.0f;

                // Get the current wave data
                WaveData currentWaveData = waves.waveList[currentWave];

                // Choose a random wave to spawn based on their weights
                int totalWeight = 0;
                foreach (Wave wave in currentWaveData.enemyInfoList)
                {
                    totalWeight += wave.weight;
                }

                int randomNumber = Random.Range(0, totalWeight);
                Wave chosenWave = null;
                foreach (Wave wave in currentWaveData.enemyInfoList)
                {
                    if (randomNumber < wave.weight)
                    {
                        chosenWave = wave;
                        break;
                    }

                    randomNumber -= wave.weight;
                }

                // Spawn the enemy
                if (chosenWave != null && currentWaveData.enemyCount > 0)
                {
                    Instantiate(chosenWave.enemy, new Vector3(0, 0, 0), Quaternion.identity);
                    enemyCount++;
                    currentWaveData.enemyCount--;
                }

                // Check if all enemies in the current wave have been spawned
                if (currentWaveData.enemyCount == 0)
                {
                    // Move to the next wave
                    print("movetoNextWave");
                    currentWave++;
                    enemyCount = 0;
                }
            }
        }
    }
}

// private void SpawnEnemies()
// {
//     // if (_timeBtwSpawn <= 0 && enemyCount > 0)
//     // {
//     //     Instantiate(enemiesToSpawn[_enemyIndex], MapManager.GetRandomPositionOnMap(), Quaternion.identity);
//     //     _timeBtwSpawn = startTimeBtwSpawn;
//     //     enemyCount--;
//     //     _enemyIndex++;
//     // }
//     // else
//     // {
//     //     _timeBtwSpawn -= Time.deltaTime;
//     // }
// }