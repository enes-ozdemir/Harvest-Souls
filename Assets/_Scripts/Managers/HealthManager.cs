using System;
using _Scripts.Managers;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] public int maxHealthCount = 5;
        private int currentHealthCount;

        private bool canTakeDamage = true;

        [SerializeField]
        private TopUIManager topUIManager;

        [SerializeField] private Image damagedEffect;

        private void Start()
        {
            BattleManager.onBattleStarted += SetupHealth;
            ResetHealth();
            topUIManager.SetupHealthUI(maxHealthCount);
        }

        private void ResetHealth()
        {
            currentHealthCount = maxHealthCount;
        }

        private void SetupHealth()
        {
            ResetHealth();
        }

        public void Heal()
        {
            currentHealthCount += 1;
        }

        public bool isDamagable()
        {
            return canTakeDamage;
        }

        public async UniTask SetInvensible(float amount)
        {
            print("SetInvensible for " + amount);
            await UniTask.Delay(TimeSpan.FromSeconds(amount));
            canTakeDamage = true;
            print("Not Invensible anymore " + amount);
        }

        public void Damaged(int amount)
        {
            print("Player is damageble = ? " + isDamagable());
            if (!isDamagable()) return;
            canTakeDamage = false;
            DamageEffect();
            print("Player damaged");
            currentHealthCount -= amount;
            if (currentHealthCount < 0) currentHealthCount = 0;
            topUIManager.SetupHealthUI(currentHealthCount);
            SetInvensible(2f);
        }

        private async UniTaskVoid DamageEffect()
        {
            damagedEffect.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            damagedEffect.gameObject.SetActive(false);
        }
    }
}

public static class TweenerExtensions
{
    public static async UniTask ToUniTask(this Tween tween)
    {
        while (tween.IsActive())
        {
            await UniTask.Yield();
        }
    }
}