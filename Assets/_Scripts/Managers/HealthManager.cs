using System;
using _Scripts.Managers;
using _Scripts.UI;
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

        private bool _canTakeDamage = true;

        [SerializeField]
        private TopUIManager topUIManager;

        [SerializeField] private Image damagedEffect;

        private void Awake()
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
            topUIManager.onHealthChanged.Invoke(currentHealthCount);
        }

        public bool isDamagable()
        {
            return _canTakeDamage;
        }

        public async UniTask SetInvensible(float amount)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(amount));
            _canTakeDamage = true;
        }

        public void Damaged(int amount)
        {
            if (!isDamagable()) return;
            _canTakeDamage = false;
            DamageEffect();
            currentHealthCount -= amount;
            if (currentHealthCount < 0) currentHealthCount = 0;
            topUIManager.onHealthChanged.Invoke(currentHealthCount);
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