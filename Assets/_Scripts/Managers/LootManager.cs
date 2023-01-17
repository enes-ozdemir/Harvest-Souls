using System;
using UnityEngine;

namespace _Scripts.Managers
{
    public class LootManager : MonoBehaviour
    {
        #region  Singleton

        public static LootManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        #endregion
        
        [SerializeField] private SkillData[] avaliableSkills;
        [SerializeField] private GameObject skillPrefab;
        public void DropSkill(Vector2 position)
        {
           // Instance()
        }
    }

    public class CollectableSkill : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            throw new NotImplementedException();
        }
    }
    
    
}