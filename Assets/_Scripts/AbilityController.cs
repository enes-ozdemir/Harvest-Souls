using System;
using System.Collections.Generic;
using _Scripts.Behaviour;
using _Scripts.Skill;
using _Scripts.SO;
using _Scripts.UI;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
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
        [FormerlySerializedAs("_abilityState")] public AbilityState abilityState = AbilityState.Ready;
 
        public Ability(SkillData skillData,float cooldownTime,float activeTime,AbilityState abilityState)
        {
            this.skillData = skillData;
            this.cooldownTime = cooldownTime;
            this.activeTime = activeTime;
            this.abilityState = abilityState;
        }
    }

    public enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }


    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private GameObject skillField;
        [SerializeField] public List<Ability> mainAbilities;
        private Ability _qAbility;
       private Ability _rAbility;

        private Camera _camera;
        private Vector3 _shootDir;
        private KeyCode[] _keycodes = new KeyCode[] {KeyCode.Q, KeyCode.R, KeyCode.E, KeyCode.Mouse0, KeyCode.Mouse1};

        public Action<Ability> onAbilityUsed;
        public Action<Ability> onAbilityCollected;

        private void Awake()
        {
            _camera = Camera.main;
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

        public void SkillCollected(SkillData skillData)
        {
            var ability = new Ability(skillData, 0, 0, AbilityState.Ready);
            onAbilityCollected.Invoke(ability);
            SetNewSkill(ability);
        }

        private void SetNewSkill(Ability ability)
        {
            if (_qAbility == null)
            {
                _qAbility = ability;
            }
            else if (_rAbility == null)
            {
                _rAbility = ability;
            }
            else
            {
                _qAbility = ability;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Skill"))
            {
                print("Skill collected");
                var skillDrop = collision.GetComponent<SkillDrop>();
                var skill = skillDrop.GetSkill();
                SkillCollected(skill);
                Destroy(skillDrop.gameObject);
            }
        }

        private void Update()
        {
            foreach (var keycode in _keycodes)
            {
                if (Input.GetKeyDown(keycode))
                {
                    switch (keycode)
                    {
                        case KeyCode.Q:
                            CastSkill(_qAbility);
                            _qAbility = null;
                            break;
                        case KeyCode.R:
                            CastSkill(_rAbility);
                            _rAbility = null;
                            break;
                        case KeyCode.E:
                            CastSkill(mainAbilities[0]);
                            break;
                        case KeyCode.Mouse0:
                            CastSkill(mainAbilities[1]);
                            break;
                        case KeyCode.Mouse1:
                            CastSkill(mainAbilities[1]);
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
            ability.abilityState = AbilityState.Active;
            ability.activeTime = ability.skillData.activeTime;
            UpdateAbilityState(ability);
        }

        private void UpdateAbilityState(Ability ability)
        {
            if (ability.abilityState == AbilityState.Active)
            {
                if (ability.activeTime > 0)
                {
                    ability.activeTime -= Time.deltaTime;
                }
                else
                {
                    ability.abilityState = AbilityState.Cooldown;
                    ability.cooldownTime = ability.skillData.cooldownTime;
                }
            }
            else if (ability.abilityState == AbilityState.Cooldown)
            {
                if (ability.cooldownTime > 0)
                {
                    ability.cooldownTime -= Time.deltaTime;
                }
                else
                {
                    ability.abilityState = AbilityState.Ready;
                }
            }
        }

        private bool CanCast(Ability ability, Vector3 mousePos)
        {
            if (ability.abilityState != AbilityState.Ready) return false;
            
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