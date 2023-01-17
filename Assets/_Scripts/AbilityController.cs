using System;
using System.Collections.Generic;
using _Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    [Serializable]
    public class Ability
    {
        public KeyCode keyCode;
        public SkillData skillData;
        public float cooldownTime;
        public float activeTime;
        public AbilityState _abilityState = AbilityState.Ready;
    }

    public enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }


    public class AbilityController : MonoBehaviour
    {
        private Vector3 _shootDir;
        [SerializeField] private GameObject skillField;
        [SerializeField] private SkillUIManager skillUIManager;
        [SerializeField] private List<Ability> activeAbilities;

        private Camera _camera;

        private void Awake()
        {
            skillUIManager.OnAbilityUsed += SetActiveAbilities;
            _camera=Camera.main;
            // foreach (var ability in abilitiesList)
            // {
            //     activeAbilities.Add(ability);
            // }
        }

        private void SetActiveAbilities(Ability ability)
        {
           // activeAbilities.Remove(ability);
        }


        private async UniTaskVoid ShowField()
        {
            skillField.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(0.8f));
            skillField.SetActive(false);
        }

        public void SkillCollected(Ability ability)
        {
            ability.keyCode = skillUIManager.SetupSkill(ability);
            activeAbilities.Add(ability);
        }


        private void Update()
        {
            var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            foreach (var ability in activeAbilities)
            {
                if (ability._abilityState == AbilityState.Ready && Input.GetKeyDown(ability.keyCode))
                {
                    if (!CanCast(ability, mousePos)) return;

                    Debug.Log($"Ability used {ability.skillData.name}");
                    ability.skillData.CastSkill(gameObject, mousePos);
                    skillUIManager.SkillUsed(ability);
                    ability._abilityState = AbilityState.Active;
                    ability.activeTime = ability.skillData.activeTime;
                }

                UpdateAbilityState(ability);
            }
        }

        private void UpdateAbilityState(Ability ability)
        {
            if (ability._abilityState == AbilityState.Active)
            {
                if (ability.activeTime > 0) 
                {
                    ability.activeTime -= Time.deltaTime;
                }
                else
                {
                    ability._abilityState = AbilityState.Cooldown;
                    ability.cooldownTime = ability.skillData.cooldownTime;
                }
            }
            else if (ability._abilityState == AbilityState.Cooldown)
            {
                if (ability.cooldownTime > 0)
                {
                    ability.cooldownTime -= Time.deltaTime;
                }
                else
                {
                    ability._abilityState = AbilityState.Ready;
                }
            }
        }


        private bool CanCast(Ability ability, Vector3 mousePos)
        {
            var maxDistance = ability.skillData.skillRange;
            if (Vector2.Distance(mousePos, transform.position) >= maxDistance)
            {
                Debug.Log("Not In Range");
                ShowField();
                return false;
            }

            return true;
        }
    }
}