using System;
using System.Collections.Generic;
using _Scripts.SO;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "BattleLevel")]
    public class BattleLevel : ScriptableObject
    {
        public List<WaveData> waveList;
        public int difficulty;
        public float timeBetweenWaves;
        public Sprite backgroundSprite;
    }
    
    [Serializable]
    public class WaveData
    {
        public List<Wave> enemyInfoList;
        public int enemyCount;
        public float timeBetweenSpawns;
    }
    
    [Serializable]
    public class Wave
    {
        public int weight; // Weight of the wave
        public GameObject enemy; // Enemy to spawn in the wave
    }
}