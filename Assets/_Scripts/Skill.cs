using UnityEngine;

namespace _Scripts
{
    public class Skill : MonoBehaviour
    {
        private SkillData skillData;
        private Vector3 _shootDir;

        public void SetSkillData(SkillData skillData) => this.skillData = skillData;

        private void Update()
        {
            if (skillData.isOnMouse)
            {
                transform.position += _shootDir * skillData.skillSpeed * Time.deltaTime;
            }
        }

        public void CastSkill()
        {
            Instantiate(skillData.skillPrefab, transform.position, Quaternion.identity);
        }
    }

    public enum SkillType
    {
        SoulBall,
        SoulRay,
        Heal
    }
}