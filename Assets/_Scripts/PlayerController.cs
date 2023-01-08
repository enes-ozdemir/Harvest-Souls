using System;
using System.Collections.Generic;
using _Scripts.SO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private HealthManager healthManager;
        [SerializeField] public PlayerData _playerData;
        [SerializeField] public BattleManager battleManager;

        public static PlayerController Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        private SpriteRenderer _spriteRenderer;

        public Action<int> onDamaged;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            onDamaged += Damaged;
        }

        private void Update()
        {
            Harvest();
        }

        private void Harvest()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                print("Harvest started");
                var collectables = GetNearbySouls();
                HarvestSouls(collectables);
            }
        }

        private void HarvestSouls(List<ICollectable> collectables)
        {
            foreach (var collectable in collectables)
            {
                var amount = collectable.Collect(transform);
                battleManager.AddSoul(amount);
                SoulManager.Instance.OnSoulCollected.Invoke();
            }
        }

        private List<ICollectable> GetNearbySouls()
        {
            var collectables = new List<ICollectable>();

            var colliders = Physics2D.OverlapCircleAll(transform.position, _playerData.collectRadious);

            foreach (var col in colliders)
            {
                var collectable = col.gameObject;

                if (!collectable.CompareTag("Soul")) continue;

                collectables.Add(collectable.GetComponent<ICollectable>());
            }

            return collectables;
        }

        private void OnDrawGizmos()
        {
            // Draw a circle around the player to represent the collect radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _playerData.collectRadious);
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

        public int GetDamage() => _playerData.damage;
        public float GetAttackSpeed() => _playerData.attackSpeed;
    }
}