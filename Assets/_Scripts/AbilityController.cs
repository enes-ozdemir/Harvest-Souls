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
        [SerializeField] private List<Ability> mainAbilities;
        [SerializeField] private Ability qAbility;
        [SerializeField] private Ability rAbility;

        private Camera _camera;

        public Action<Ability> onAbilityUsed;
        public Action<Ability> onAbilityCollected;


        private void Awake()
        {
            _camera = Camera.main;
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
            onAbilityCollected.Invoke(ability);
            //activeAbilities.Add(ability);
        }

        KeyCode[] keycodes = new KeyCode[] {KeyCode.Q, KeyCode.R, KeyCode.E, KeyCode.Mouse0, KeyCode.Mouse1};

        private void Update()
        {
            foreach (var keycode in keycodes)
            {
                if (Input.GetKeyDown(keycode))
                {
                    switch (keycode)
                    {
                        case KeyCode.Q:
                            CastSkill(qAbility);
                            break;
                        case KeyCode.R:
                            CastSkill(rAbility);
                            break;
                        case KeyCode.E:
                            CastSkill(mainAbilities[0]);
                            break;
                        case KeyCode.Mouse0:
                            CastSkill(mainAbilities[1]);
                            break;
                        case KeyCode.Mouse1:
                            CastSkill(mainAbilities[2]);
                            break;
                    }
                }
            }
        }

        private void CastSkill(Ability ability)
        {
            var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (!CanCast(ability, mousePos)) return;

            Debug.Log($"Ability used {ability.skillData.name}");
            ability.skillData.CastSkill(gameObject, mousePos);
            onAbilityUsed.Invoke(ability);
            ability._abilityState = AbilityState.Active;
            ability.activeTime = ability.skillData.activeTime;
            UpdateAbilityState(ability);
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
            if (ability._abilityState != AbilityState.Ready) return false;
            
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