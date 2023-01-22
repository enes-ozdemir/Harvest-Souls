using System;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.Util;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Managers
{
    public class SoulManager : MonoBehaviour
    {
        #region  Singleton

        public static SoulManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        #endregion
        
        [SerializeField] private GameObject soulPrefab;
        public List<ICollectable> _collectableList = new();

        public Action<int> OnSoulCollected;

        public void DropSoul(int amount, Vector2 position)
        {
            var soulObject = ObjectPooler.Instance
                .SpawnFromPool("Soul", position, quaternion.identity);
            var collectable = soulObject.GetComponent<Soul>();
            collectable.Setup(amount);
            _collectableList.Add(collectable);
        }
        
        public void AddSoul()
        {
            print("Soul added " + 1);
            PlayerStats.soulAmount += 1;
            //  PlayerPrefs.SetInt("soulAmount",amount);
        }
    }
}