using System;
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
        [SerializeField]private SkillData mainSkillAbility;
        [SerializeField]private SkillData leftClickAbility;
        [SerializeField]private SkillData rightClickAbility;

        #endregion

        [Space] [SerializeField] private UISkill firstCollectedSkill;
        [SerializeField] private UISkill secondCollectedSkill;
        private Ability _firstSkill;
        private Ability _secondSkill;

        private bool _isIndexChanged;

        public Action<Ability> OnAbilityUsed;

        public KeyCode SetupSkill(Ability ability)
        {
            UISkill collectedSkill;
            if (!_isIndexChanged)
            {
                collectedSkill = firstCollectedSkill;
                _firstSkill = ability;
            }
            else
            {
                collectedSkill = secondCollectedSkill;
                _secondSkill = ability;
            }

            collectedSkill.skillImage.gameObject.SetActive(true);
            collectedSkill.skillImage = ability.skillData.skillImage;
            _isIndexChanged = !_isIndexChanged;

            return _isIndexChanged ? KeyCode.Q : KeyCode.R;
        }

        public void SkillUsed(Ability ability)
        {
            OnAbilityUsed.Invoke(ability);

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
            else if (mainSkillAbility == ability.skillData)
            {
                usedSkill = mainSkill;
            }
            else if (leftClickAbility == ability.skillData)
            {
                usedSkill = leftClickSkill;
            }
            else if (rightClickAbility == ability.skillData)
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
            uiSkillComponent.ToggleCooldown();
            uiSkillComponent.skillImage.fillAmount = 1;
            float time = 0;
            while (time < cooldown)
            {
                var amount = Mathf.Lerp(1, 0, time / cooldown);
                await UniTask.Delay(1000);
                uiSkillComponent.SetCooldown(cooldown,amount);
                time += 1;
            }
        }
    }
}