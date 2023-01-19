using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.UI
{
    public class SkillUIManager : MonoBehaviour
    {
        #region MainAbilities

        [SerializeField] private UISkill mainSkill;
        [SerializeField] private UISkill leftClickSkill;
        [SerializeField] private UISkill rightClickSkill;

        #endregion

        [Space] [SerializeField] private UISkill firstCollectedSkill;
        [SerializeField] private UISkill secondCollectedSkill;
        private Ability _firstSkill;
        private Ability _secondSkill;
        [SerializeField] private Ability mainSkillAbility;
        [SerializeField] private Ability leftClickAbility;
        [SerializeField] private Ability rightClickAbility;

        private List<Ability> usableAbilitiesList;
        [SerializeField] private List<Ability> mainAbilitiesList;

        private bool _isIndexChanged;
        [SerializeField] private AbilityController _abilityController;


        private void Awake()
        {
            _abilityController.onAbilityUsed += SkillUsed;
            _abilityController.onAbilityCollected += SetupSkill;
            usableAbilitiesList.Add(mainSkillAbility);
            usableAbilitiesList.Add(leftClickAbility);
            usableAbilitiesList.Add(rightClickAbility);
            usableAbilitiesList.Add(_firstSkill);
            usableAbilitiesList.Add(_secondSkill);
        }

        private void SetupSkill(Ability ability)
        {
            UISkill collectedSkill;
            if (!_isIndexChanged)
            {
                collectedSkill = firstCollectedSkill;
                _firstSkill = ability;
                collectedSkill.keyCode = KeyCode.Q;
            }
            else
            {
                collectedSkill = secondCollectedSkill;
                _secondSkill = ability;
                collectedSkill.keyCode = KeyCode.R;
            }

            collectedSkill.gameObject.SetActive(true);
            collectedSkill.skillImage = ability.skillData.skillImage;
            _isIndexChanged = !_isIndexChanged;
        }

        private void SkillUsed(Ability ability)
        {
            UISkill usedSkill = null;
            if (_firstSkill == ability)
            {
                usedSkill = firstCollectedSkill;
                _firstSkill = null;
            }
            else if (_secondSkill == ability)
            {
                usedSkill = secondCollectedSkill;
                _secondSkill = null;
            }
            else if (mainSkillAbility == ability)
            {
                usedSkill = mainSkill;
            }
            else if (leftClickAbility == ability)
            {
                usedSkill = leftClickSkill;
            }
            else if (rightClickAbility == ability)
            {
                usedSkill = rightClickSkill;
            }

            if (usedSkill != null)
            {
                usedSkill.skillImage.gameObject.SetActive(true);
                usedSkill.skillImage = ability.skillData.skillImage;

                SetCooldown(usedSkill, ability.cooldownTime);
            }
        }

        private async void SetCooldown(UISkill uiSkillComponent, float cooldown)
        {
            Debug.Log("Cooldown set");
            uiSkillComponent.ToggleCooldown();
            uiSkillComponent.skillImage.fillAmount = 1;
            float time = 0;
            while (time < cooldown)
            {
                var amount = Mathf.Lerp(1, 0, time / cooldown);
                await UniTask.Delay(1000);
                uiSkillComponent.SetCooldown(cooldown, amount);
                time += 1;
            }
        }
    }
}