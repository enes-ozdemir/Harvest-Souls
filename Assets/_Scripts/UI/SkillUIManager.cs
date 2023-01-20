using System.Collections.Generic;
using System.Linq;
using _Scripts.SO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.UI
{
    public class SkillUIManager : MonoBehaviour
    {
        #region MainAbilities

        [SerializeField] private List<UISkill> mainUISkillList;
        private List<Ability> _mainAbilitiesList;

        #endregion

        [Space] [SerializeField] private List<UISkill> uiSkillList;
        [SerializeField] private List<Ability> avaliableAbilitiesList;
        [SerializeField] private AbilityController abilityController;
        private int _maxAbilities = 2;
        private KeyCode[] _keycodes = new KeyCode[] {KeyCode.Q, KeyCode.R};


        private void Awake()
        {
            _mainAbilitiesList = abilityController.mainAbilities;
            abilityController.onAbilityUsed += SkillUsed;
            abilityController.onAbilityCollected += SetupSkill;
        }

        private void SetupSkill(Ability ability)
        {
            if (avaliableAbilitiesList.Count == _maxAbilities)
            {
                avaliableAbilitiesList.RemoveAt(0);
            }

            avaliableAbilitiesList.Add(ability);
           
            SetUI();
        }

        private void SetUI()
        {
            for (var index = 0; index < avaliableAbilitiesList.Count; index++)
            {
                var ability = avaliableAbilitiesList[index];
                uiSkillList[index].Setup(ability.skillData.skillImage, _keycodes[index]);
            }
        }

        private void SkillUsed(Ability ability)
        {
            if (IsCollectableSkillUsed(ability)) return;

            CheckForMainAbilityUsage(ability);
        }

        private void CheckForMainAbilityUsage(Ability ability)
        {
            UISkill usedSkill = null;
            for (var index = 0; index < _mainAbilitiesList.Count; index++)
            {
                var mainAbility = _mainAbilitiesList[index];
                if (mainAbility == ability)
                {
                    usedSkill = mainUISkillList[index];
                }
            }

            if (usedSkill != null)
            {
                usedSkill.skillImage.gameObject.SetActive(true);
                usedSkill.skillImage.sprite = ability.skillData.skillImage;

                SetCooldown(usedSkill, ability);
            }
        }

        private bool IsCollectableSkillUsed(Ability ability)
        {
            foreach (var avAbility in avaliableAbilitiesList.Where(avAbility => ability == avAbility))
            {
                avaliableAbilitiesList.Remove(avAbility);
                SetUI();
                return true;
            }

            return false;
        }

        private async void SetCooldown(UISkill uiSkillComponent, Ability ability)
        {
            var cooldown = ability.cooldownTime;
            uiSkillComponent.ToggleCooldown();
            uiSkillComponent.skillImage.fillAmount = 1;
            float time = 0;
            while (time <= cooldown)
            {
                ability.abilityState = AbilityState.Cooldown;
                var amount = Mathf.Lerp(1, 0, time / cooldown);
                uiSkillComponent.SetCooldown(cooldown - time, amount);
                time += 1;
                await UniTask.Delay(1000);
            }

            ability.abilityState = AbilityState.Ready;
            ;
        }
    }
}