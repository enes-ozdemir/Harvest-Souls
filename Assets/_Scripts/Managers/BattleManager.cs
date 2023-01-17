using System;
using _Scripts.Data;
using _Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Managers
{
    public class BattleManager : MonoBehaviour
    {
        public static Action onBattleStarted;
        public static Action onBattleEnded;

        private int currentLevel;
        private int currentWave;

        [FormerlySerializedAs("allWaves")] [SerializeField] private BattleLevel[] battleLevels;

        [SerializeField] private EnemySpawnManager enemySpawnManager;
        [SerializeField] private GameObject backgroundSprite;

        [SerializeField] private GameObject goBackObject;
        [SerializeField] private GameObject continueObject;


        public void Start()
        {
            Setup();
            print($"current wave"+ currentWave);
            enemySpawnManager.Setup(battleLevels[currentWave], currentLevel);
            backgroundSprite.GetComponent<SpriteRenderer>().sprite = battleLevels[currentLevel].backgroundSprite;
            currentLevel = PlayerStats.currentLevel;
            currentWave = PlayerStats.currentWave;
            // enemySpawnManager.Setup(currentLevel);
            //  backgroundSprite = maps[currentLevel];
        }

        private void Setup()
        {
            goBackObject.SetActive(false);
            continueObject.SetActive(false);
            enemySpawnManager.onWaveEnded += SpawnEndGameObjects;
            onBattleStarted.Invoke();
        }

        private void SpawnEndGameObjects()
        {
            goBackObject.SetActive(true);
            continueObject.SetActive(true);
        }

        public void AddSoul(int amount)
        {
            print("Soul added " + amount);
            PlayerStats.soulAmount += amount;

            //  PlayerPrefs.SetInt("soulAmount",amount);
        }

        private void OnDisable()
        {
            onBattleEnded.Invoke();
        }
    }
}