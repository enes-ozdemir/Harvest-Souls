using _Scripts.SO;
using UnityEngine;

namespace _Scripts.Skill
{
    [CreateAssetMenu]
    public class Heal : SkillData
    {
        public override void CastSkill(GameObject player, Vector3 mousePos)
        {
            Debug.Log("Heal start");
            player.GetComponent<HealthManager>().Heal();
            Instantiate(skillPrefab, player.transform.position, Quaternion.identity);
        }
    }
}