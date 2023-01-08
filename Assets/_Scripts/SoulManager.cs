using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts
{
    public class SoulManager : MonoBehaviour
    {
        public static SoulManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        [SerializeField] private GameObject soulPrefab;
        private List<ICollectable> _collectableList = new();
        [SerializeField] private BattleManager _battleManager;

        public Action OnSoulCollected;

        public void DropSoul(int amount, Vector2 position)
        {
            var soulObject = Instantiate(soulPrefab, position, quaternion.identity);
            var collectable = soulObject.GetComponent<Soul>();
            collectable.Setup(amount);
            _collectableList.Add(collectable);
        }

      
    }
}