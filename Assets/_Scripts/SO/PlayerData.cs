using UnityEngine;

namespace _Scripts.SO
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public int damage;
        public int health;
        public float speed;
        public int invensibleTime;
        public float collectRadious;
        public float attackSpeed;
    }
}