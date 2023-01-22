using _Scripts.SO;
using UnityEngine;

namespace _Scripts.Skill
{
    [CreateAssetMenu]
    public class Teleport : SkillData
    {
        public override void CastSkill(GameObject player,Vector3 mousePos)
        {
            Debug.Log("Teleport start");

            var teleportPrefab = Instantiate(skillPrefab, player.transform.position, Quaternion.identity);
            var secondTeleportPrefab = Instantiate(skillPrefab, mousePos, Quaternion.identity);
            player.transform.position = new Vector3(mousePos.x,0,mousePos.y);
            Destroy(teleportPrefab,1f);
            Destroy(secondTeleportPrefab,1.2f);
        }
    }
}