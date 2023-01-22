using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Managers
{
    public class SkillManager : MonoBehaviour
    {
        public List<Ability> allAbilities;

        public Ability GetAbility(string abilityName)
        {
            Ability ability = null;

            foreach (var skill in allAbilities.Where(skill => skill.skillData.name.Equals(abilityName)))
            {
                ability = skill;
            }

            return ability;
        }
    }
}