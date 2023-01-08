using System;
using UnityEngine;

namespace _Scripts
{
    public class BattleManager : MonoBehaviour
    {
        public static Action onBattleStarted;
        public static Action onBattleEnded;
        public GameData gameData;
        public int currentLevel;
        public Sprite[] maps;

        [SerializeField] private EnemySpawnManager enemySpawnManager;
        [SerializeField] private Sprite backgroundSprite;

        public void Start()
        {
            onBattleStarted.Invoke();
           // enemySpawnManager.Setup(currentLevel);
          //  backgroundSprite = maps[currentLevel];
        }

        public void AddSoul(int amount)
        {
            print("Soul added " + amount);
            gameData.soulCount += amount;
        }

        private void OnDisable()
        {
            onBattleEnded.Invoke();
        }
    }
}