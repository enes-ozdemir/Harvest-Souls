using System;
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
                    print("Enemy attacking");
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
        // private void SpawnParticle(PlayerAimController.OnShootEventArgs args)
        // {
        //     print("Spawned");
        //     var _projectileTransform = Instantiate(projectile, args.gunEndPointPosition, Quaternion.identity);
        //     var shootDir = (args.shootPosition - args.gunEndPointPosition).normalized;
        //     _projectileTransform.GetComponent<Projectile>().Setup(shootDir, playerController.GetDamage());
        // }

        private void Attack()
        {
            //play attack anim
            characterAnimationController.PlayAnimation(AnimationType.Attack);
            if (enemyData.isRanged)
            {
            }
            else
            {
                target.GetComponent<PlayerController>().onDamaged.Invoke(enemyData.damage);
            }
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
                Destroy(projectile.gameObject);
            }
        }

        private void GotDamaged(int damageAmount)
        {
            currentHealth -= damageAmount;
            DoDamagedEffect();
            print("Enemy got damaged remaining health:" + currentHealth);

            if (currentHealth < 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        private void Die()
        {
            soulManager.DropSoul(enemyData.soulAmount, transform.position);
            characterAnimationController.PlayAnimation(AnimationType.Death);
            _isDead = true;
        }

        private void RemoveCorpse()
        {
            if (!_isDead) return;
            _spriteRenderer.DOFade(0, 0.5f);
            Instantiate(enemyData.deathPrefab, transform);
        }


        private async UniTaskVoid DoDamagedEffect()
        {
            var duration = 0.1f;
            //add hit particle
            //add dead effect

            _spriteRenderer.DOColor(Color.red, 1).SetEase(Ease.Linear);
            _spriteRenderer.DOFade(0.5f, 1f).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _spriteRenderer.DORewind();
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        private void OnDestroy()
        {
            soulManager.OnSoulCollected -= RemoveCorpse;
        }
    }
}