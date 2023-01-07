using System;
using _Scripts.SO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public Action<int> onDamaged;
        [SerializeField] private HealthManager healthManager;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] public PlayerData _playerData;


        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            onDamaged += Damaged;
        }

        private void Damaged(int damageAmount)
        {
            healthManager.Damaged(damageAmount);
            PlayDamagedAnim();
        }

        private async UniTaskVoid PlayDamagedAnim()
        {
            var duration = 0.1f;

            for (int i = 0; i < 10; i++)
            {
                _spriteRenderer.DOFade(0.5f, duration);
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                _spriteRenderer.DOFade(1f, duration);
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
            }
        }
    }
}