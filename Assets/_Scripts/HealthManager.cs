using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] public int maxHealthCount = 5;
        private int currentHealthCount;

        private bool canTakeDamage = true;
        [SerializeField] private HealthUIManager _healthUIManager;

        [SerializeField] private Image damagedEffect;

        private void Start()
        {
            BattleManager.onBattleStarted += SetupHealth;
            ResetHealth();
            _healthUIManager.SetupHealthUI(maxHealthCount);
        }

        private void ResetHealth()
        {
            currentHealthCount = maxHealthCount;
        }

        private void SetupHealth()
        {
            ResetHealth();
        }

        public bool isDamagable()
        {
            return canTakeDamage;
        }

        public async UniTask SetInvensible(float amount)
        {
            print("SetInvensible for " + amount);
            canTakeDamage = false;
            await UniTask.Delay(TimeSpan.FromSeconds(amount));
            canTakeDamage = true;
            print("Not Invensible anymore " + amount);
        }

        public void Damaged(int amount)
        {
            DamageEffect();
            print("Player is damageble = ? " + isDamagable());
            if (!isDamagable()) return;
            print("Player damaged");
            currentHealthCount -= amount;
            if (currentHealthCount < 0) currentHealthCount = 0;
            _healthUIManager.SetupHealthUI(currentHealthCount);
            SetInvensible(2f);
        }

        private async UniTaskVoid DamageEffect()
        {
            damagedEffect.gameObject.SetActive(true);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(damagedEffect.DOFade(1, 1F));
            sequence.Append(damagedEffect.DOFade(0.5F, 0.5f));
            sequence.Append(damagedEffect.DOFade(1, 1F));
            sequence.Append(damagedEffect.DOFade(0, 1F));
            sequence.OnComplete(() => damagedEffect.gameObject.SetActive(false));

            await sequence.ToUniTask();
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