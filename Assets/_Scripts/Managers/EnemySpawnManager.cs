using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Managers
{
    public class EnemySpawnManager : MonoBehaviour
    {
        private BattleLevel currentWaveObject;

        public int waveNumber;
        public int currentWaveNumber;
        private int _currentWaveCount;
        private float spawnTimer = 0.0f;
        private float _passedTime = 0.0f;
        private int currentEnemyCount = 0; // Current number of enemies

        private WaveData _waveData;

        private bool isSetupDone;
        public List<Enemy> enemyList = new();

        public void SetWave(BattleLevel wave)
        {
            currentWaveObject = wave;
            currentWaveNumber = 0;
            waveNumber = 0;
        }

        public void Setup(BattleLevel gameDataCurrentWave, int waveNumber)
        {
            currentWaveObject = gameDataCurrentWave;
            _currentWaveCount = currentWaveObject.waveList[waveNumber].enemyCount;
            spawnTimer = currentWaveObject.waveList[waveNumber].timeBetweenSpawns;
            currentWaveNumber = waveNumber;
            _waveData = currentWaveObject.waveList[waveNumber];
            isSetupDone = true;
        }

        private void Update()
        {
            if (!isSetupDone) return;
            //todo instantiate this at the begining
            SpawnCurrentWave();
        }

        public Action onWaveEnded;

        private GameObject SelectRandomEnemy()
        {
            int randomIndex = Random.Range(0, _waveData.enemyInfoList.Count);
            var enemy = _waveData.enemyInfoList[randomIndex].enemy;
            return enemy;
        }

        private void SpawnCurrentWave()
        {
            if (_currentWaveCount == 0) return; // exit method if no more enemies to spawn

            _passedTime += Time.deltaTime;

            if (_passedTime >= spawnTimer)
            {
                _passedTime = 0.0f;

                var enemyPrefab = SelectRandomEnemy();
                var spawnPosition = MapManager.Instance.GetRandomPositionOnMap();
                var spawnRotation = Quaternion.identity;

                var enemyGameObject = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
                var enemy = enemyGameObject.GetComponent<Enemy>();
                enemyList.Add(enemy);
                enemy.onDead += EnemyDied;
                _currentWaveCount--;
                currentEnemyCount++;
            }
        }


        public void EnemyDied(Enemy enemy)
        {
            currentEnemyCount--;
            enemyList.Remove(enemy);
        }
    }
}