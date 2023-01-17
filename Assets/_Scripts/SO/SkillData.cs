using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu]
    public class SkillData : ScriptableObject
    {
        public SkillType skillType;
        public float effectArea;
        public float skillSpeed;
        public float skillDamage;
        public float skillCount;
        public GameObject skillPrefab;
        public GameObject explosivePrefab;
        public Sprite skillSprite;
        public bool isOnMouse;
    }
}