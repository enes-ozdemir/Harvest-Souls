using UnityEngine;

namespace _Scripts.SO
{
    [CreateAssetMenu]
    public class EnemyData : ScriptableObject
    {
        public int health;
        public float speed;
        public float attackSpeed;
        public int damage;
        public int soulAmount;
        public bool isRanged;
        public float range;
        public GameObject deathPrefab;
        public GameObject projectilePrefab;

    }
}