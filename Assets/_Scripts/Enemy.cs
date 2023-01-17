using System;
using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.SO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class Enemy : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;
        [SerializeField] public EnemyData enemyData;
        private SoulManager soulManager;

        private CharacterAnimationController characterAnimationController;
        private SpriteRenderer _spriteRenderer;

        public Transform target;

        private float _passedTime;
        private bool _isDead;

        public int priority = 1;

        public Action<Enemy> onDead;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            soulManager = SoulManager.Instance;
            soulManager.OnSoulCollected += RemoveCorpse;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            characterAnimationController = GetComponent<CharacterAnimationController>();
            maxHealth = enemyData.health;
            currentHealth = maxHealth;
            _spriteRenderer.sortingOrder = Random.Range(20, 98);
            target = PlayerController.Instance.transform;
        }

        private void Update()
        {
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            if (_isDead) return;
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance < enemyData.range)
            {
                if (_passedTime >= enemyData.attackSpeed)
                {
                    _passedTime = 0;
                    Attack();
                }
                else
                {
                    characterAnimationController.PlayAnimation(AnimationType.Idle);
                }
            }
            else
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, target.position, enemyData.speed * Time.deltaTime);
                characterAnimationController.PlayAnimation(AnimationType.Move);
            }


            if (_passedTime < enemyData.attackSpeed)
            {
                _passedTime += Time.deltaTime;
            }
        }

        private void Attack()
        {
            //play attack anim
            characterAnimationController.PlayAnimation(AnimationType.Attack);
            if (enemyData.isRanged)
            {
                SpawnParticle();
            }
            else
            {
                target.GetComponent<PlayerController>().onDamaged.Invoke(enemyData.damage);
            }
        }

        private void SpawnParticle()
        {
            var projectileTransform = Instantiate(enemyData.projectilePrefab, transform.position, Quaternion.identity);
            print("Spawned");
            var shootDir = CalculateDir();
            projectileTransform.GetComponent<Projectile>().Setup(shootDir, enemyData.damage);
        }

        private Vector3 CalculateDir()
        {
            var random = Random.Range(0, 10);
            Vector3 dir;
            if (random < 5)
                dir = (PlayerController.Instance.transform.position - transform.position).normalized;
            else if (random > 8)
                dir = (PlayerController.Instance.transform.position + new Vector3(10, 10) - transform.position)
                    .normalized;
            else
                dir = (PlayerController.Instance.transform.position + new Vector3(-10, -10) - transform.position)
                    .normalized;

            return dir;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isDead) return;
            if (collision.CompareTag("Projectile"))
            {
                var projectile = collision.GetComponent<Projectile>();
                if (projectile == null) return;
                var damage = projectile.projectileDamage;
                GotDamaged(damage);
                projectile.DestroyIt();
            }
        }

        private void GotDamaged(int damageAmount)
        {
            currentHealth -= damageAmount;
            DoDamagedEffect();

            if (currentHealth < 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            soulManager.DropSoul(enemyData.soulAmount, transform.position);
            characterAnimationController.PlayAnimation(AnimationType.Death);
        }

        private void RemoveCorpse(int amount)
        {
            if (!_isDead) return;
            _spriteRenderer.DOFade(0, 0.5f);
            var deathPrefab = Instantiate(enemyData.deathPrefab, transform);
            Destroy(gameObject);
            Destroy(deathPrefab, 1f);
        }

        private async UniTaskVoid DoDamagedEffect()
        {
            //characterAnimationController.PlayAnimation(AnimationType.Hurt);
            var duration = 0.5f;
            _spriteRenderer.DOFade(0.7f, duration).SetEase(Ease.Linear).SetId("FadeTween");
            var normalColor = _spriteRenderer.color;
            _spriteRenderer.DOColor(Color.red, duration);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _spriteRenderer.DOColor(normalColor, duration);
            _spriteRenderer.DOFade(1, duration).SetEase(Ease.Linear).SetId("FadeTween");
        }


        private void OnDestroy()
        {
            soulManager.OnSoulCollected -= RemoveCorpse;
        }
    }
}