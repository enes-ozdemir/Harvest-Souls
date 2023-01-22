using System;
using _Scripts.Managers;
using _Scripts.Skill;
using _Scripts.SO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private HealthManager healthManager;
        [SerializeField] public BattleManager battleManager;
        [SerializeField] private PlayerInfoData _playerInfoData;

        #region Singleton

        public static PlayerController Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        #endregion

        private SpriteRenderer _spriteRenderer;
        public Action<int> onDamaged;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            onDamaged += Damaged;
        }

        private void OnDrawGizmos()
        {
            // Draw a circle around the player to represent the collect radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, PlayerData.collectRadious);
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

        public int GetDamage() => PlayerData.damage;
        public float GetAttackSpeed() => PlayerData.attackSpeed;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("EnemyProjectile"))
            {
                var projectile = collision.GetComponent<Projectile>();
                if (projectile == null) return;
                var damage = projectile.projectileDamage;
                Damaged(damage);
                projectile.gameObject.SetActive(false);
            }
        }
    }
}