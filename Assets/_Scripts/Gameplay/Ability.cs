using System;
using _Scripts.Controllers;
using _Scripts.SO;

namespace _Scripts
{
    [Serializable]
    public class Ability
    {
        public SkillData skillData;
        public float cooldownTime;
        public AbilityState abilityState;

        public Ability(SkillData skillData, float cooldownTime)
        {
            this.skillData = skillData;
            this.cooldownTime = cooldownTime;
            abilityState = AbilityState.Ready;
        }
    }
}