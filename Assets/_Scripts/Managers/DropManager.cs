using System.Collections.Generic;
using _Scripts.Behaviour;
using _Scripts.Skill;
using _Scripts.SO;
using _Scripts.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Managers
{
    public class DropManager : MonoBehaviour
    {
        #region Singleton

        public static DropManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        #endregion

        public List<SkillData> skillDataList;

        public void DropSkill(Vector3 position)
        {
            var chance = Random.Range(0, 15);
            //if(chance==0)
            if (chance > 0)
            {
                var randomSkill = Random.Range(0, skillDataList.Count);
                var drop = ObjectPooler.Instance.SpawnFromPool("SkillDrop", position + new Vector3(0, 2f, 0),
                    Quaternion.identity);
                drop.GetComponent<SkillDrop>().SetupDrop(skillDataList[randomSkill]);
            }
        }
    }
}