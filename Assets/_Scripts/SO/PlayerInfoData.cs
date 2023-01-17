using UnityEngine;

namespace _Scripts.SO
{
    [CreateAssetMenu]
    public class PlayerInfoData : ScriptableObject
    {
        public int health;
        public float speed;
        public float attackSpeed;
        public int damage;
        public GameObject deathPrefab;
        public GameObject projectilePrefab;
        public GameObject teleportPrefab;
        public GameObject healPrefab;
        public GameObject[] skillPrefabs;
    }
}