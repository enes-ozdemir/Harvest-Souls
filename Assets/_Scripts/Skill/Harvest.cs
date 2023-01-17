using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Skill
{
    [CreateAssetMenu]
    public class Harvest : SkillData
    {
        public override void CastSkill(GameObject player,Vector3 mousePos)
        {
            Debug.Log("Harvest start");

            var collectables = GetNearbySouls(player.transform.position);
            HarvestSouls(collectables,player.transform);

            foreach (var soul in collectables)
            {
                SoulManager.Instance._collectableList.Remove(soul);
            }
        }
        
        private void HarvestSouls(List<ICollectable> collectables, Transform playerTransform)
        {
            foreach (var collectable in collectables)
            {
                var amount = collectable.Collect(playerTransform);
                SoulManager.Instance.OnSoulCollected.Invoke(amount);
            }
        }

        private List<ICollectable> GetNearbySouls(Vector3 playerPos)
        {
            var collectables = new List<ICollectable>();

            var colliders = Physics2D.OverlapCircleAll(playerPos, PlayerData.collectRadious);

            foreach (var col in colliders)
            {
                var collectable = col.gameObject;

                if (!collectable.CompareTag("Soul")) continue;

                collectables.Add(collectable.GetComponent<ICollectable>());
            }

            Debug.Log("Found" + collectables.Count + " soul");
            return collectables;
        }
    }
}