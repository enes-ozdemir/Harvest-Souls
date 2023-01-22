using System;
using System.Collections.Generic;
using _Scripts.Behaviour;
using _Scripts.SO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Controllers
{
    public enum AbilityState
    {
        Ready,
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

        private KeyCode[] _keycodes = new KeyCode[] {KeyCode.Q, KeyCode.R, KeyCode.E, KeyCode.Mouse1};
        private List<Ability> allAbilities = new();

        public Action<Ability> onAbilityUsed;
        public Action<Ability> onAbilityCollected;

        private void Awake()
        {
            _camera = Camera.main;
            foreach (var ability in mainAbilities) allAbilities.Add(ability);
        }

        private void Update()
        {
            UpdateMainAbilityState();
            CheckForPlayerInput();
        }

        private void UpdateMainAbilityState()
        {
            foreach (var ability in mainAbilities)
            {
                if (ability.abilityState == AbilityState.Cooldown)
                {
                    if (ability.cooldownTime > 0) ability.cooldownTime -= Time.deltaTime;
                    else ability.abilityState = AbilityState.Ready;
                }
            }
        }

        private void CheckForPlayerInput()
        {
            foreach (var keycode in _keycodes)
            {
                if (Input.GetKeyDown(keycode))
                {
                    switch (keycode)
                    {
                        case KeyCode.Q:
                            if(_qAbility==null) return;
                            CastSkill(_qAbility);
                            RemoveSkillFromBar(_qAbility);
                            _qAbility = null;
                            break;
                        case KeyCode.R:
                            if(_rAbility==null) return;
                            CastSkill(_rAbility);
                            RemoveSkillFromBar(_rAbility);
                            _rAbility = null;
                            break;
                        case KeyCode.E:
                            CastSkill(mainAbilities[0]);
                            break;
                        case KeyCode.Mouse1:
                            CastSkill(mainAbilities[1]);
                            break;
                    }
                }
            }
        }

        private void SkillCollected(SkillData skillData)
        {
            var ability = new Ability(skillData, skillData.cooldownTime);
            onAbilityCollected.Invoke(ability);
            SetNewSkill(ability);
        }

        private void SetNewSkill(Ability ability)
        {
            if (allAbilities.Contains(_qAbility) && !allAbilities.Contains(_rAbility))
            {
                _rAbility = ability;
                allAbilities.Insert(1,_rAbility);
            }
            else
            {
                _qAbility = ability;
                allAbilities.Insert(0,_qAbility);
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


        private void CastSkill(Ability ability)
        {
            var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (!CanCast(ability, mousePos)) return;
            Debug.Log($"Ability used {ability.skillData.name}");
            ability.skillData.CastSkill(gameObject, mousePos);
            onAbilityUsed.Invoke(ability);

            if (!mainAbilities.Contains(ability))
            {
                RemoveSkillFromBar(ability);
            }
            else
            {
                ability.abilityState = AbilityState.Cooldown;
            }
        }

        private void RemoveSkillFromBar(Ability ability)
        {
            allAbilities.Remove(ability);
        }

        private bool CanCast(Ability ability, Vector3 mousePos)
        {
            if (ability.abilityState != AbilityState.Ready)
            {
                print("Can cast false");
                return false;
            }

            print("Can cast true");

            if (ability.skillData.isOnMouse)
            {
                var maxDistance = ability.skillData.skillRange;

                if (Vector2.Distance(mousePos, transform.position) >= maxDistance)
                {
                    Debug.Log("Not In Range");
                    ShowField(ability.skillData.skillRange);
                    return false;
                }
            }

            return true;
        }

        private async UniTaskVoid ShowField(float skillDataSkillRange)
        {
            skillField.SetActive(true);
            skillField.transform.localScale =
                new Vector3(skillDataSkillRange, skillDataSkillRange, skillDataSkillRange);
            await UniTask.Delay(TimeSpan.FromSeconds(0.8f));
            skillField.SetActive(false);
        }
    }
}