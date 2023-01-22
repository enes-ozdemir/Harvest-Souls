using _Scripts.SO;
using UnityEngine;

namespace _Scripts.Skill
{
    [CreateAssetMenu]
    public class SoulBomb : SkillData
    {
        public override void CastSkill(GameObject player, Vector3 mousePos)
        {
            Debug.Log("SoulBomb start");
            Instantiate(skillPrefab, player.transform.position, Quaternion.identity);
        }
    }
}