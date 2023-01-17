using System;
using System.Collections.Generic;
using _Scripts.Managers;
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
        [SerializeField] private GameObject skillField;
        

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

        private void Update()
        {
            CheckForHarvest();
            CheckForTeleport();
        }

        private async UniTask CheckForTeleport()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("Teleport start");
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(mousePos, transform.position) >= 20f)
                {
                    ShowField();
                    return;
                }
                
                var teleportPrefab=Instantiate(_playerInfoData.teleportPrefab, transform.position, Quaternion.identity);
                var secondTeleportPrefab=Instantiate(_playerInfoData.teleportPrefab, mousePos, Quaternion.identity);
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                transform.position = mousePos;
                Destroy(teleportPrefab,0.6f);
                Destroy(secondTeleportPrefab,0.6f);
            }
        }

        private async UniTaskVoid ShowField()
        {
            skillField.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(0.8f));
            skillField.SetActive(false);
        }

        private void CheckForHarvest()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                print("Harvest started");
                var collectables = GetNearbySouls();
                HarvestSouls(collectables);

                foreach (var soul in collectables)
                {
                    SoulManager.Instance._collectableList.Remove(soul);
                }
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

            var colliders = Physics2D.OverlapCircleAll(transform.position, PlayerData.collectRadious);

            foreach (var col in colliders)
            {
                var collectable = col.gameObject;

                if (!collectable.CompareTag("Soul")) continue;

                collectables.Add(collectable.GetComponent<ICollectable>());
            }

            print("Found" + collectables.Count + " soul");
            return collectables;
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
            print("Got hit");
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