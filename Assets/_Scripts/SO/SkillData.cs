﻿using UnityEngine;

namespace _Scripts.SO
{
    [CreateAssetMenu]
    public class SkillData : ScriptableObject
    {
        public string name;
        public Sprite skillImage;
        public float effectArea;
        public float skillSpeed;
        public float skillDamage;
        public float skillCount;
        public float skillRange;
        public GameObject skillPrefab;
        public GameObject explosivePrefab;
        public Sprite skillSprite;
        public bool isOnMouse;
        public float cooldownTime;
        public float activeTime;

        public virtual void CastSkill(GameObject player, Vector3 mousePos) {}
        public virtual void AddSkillEffect(GameObject player) {}
    }
   
}