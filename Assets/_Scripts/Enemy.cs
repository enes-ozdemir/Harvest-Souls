using System;
using UnityEngine;

namespace _Scripts
{
    public class Enemy : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;
        [SerializeField] public EnemyData enemyData;

        private CharacterAnimationController characterAnimationController;

        public Transform target;
        public PlayerController playerController;

        private float _passedTime;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            characterAnimationController = GetComponent<CharacterAnimationController>();
            playerController = GetComponent<PlayerController>();
            maxHealth = enemyData.health;
            currentHealth = maxHealth;
            
        }

        private void Update()
        {
            MoveToTarget();
        }

        private void MoveToTarget()
        {
        
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

        private void Attack()
        {
            //play attack anim
            characterAnimationController.PlayAnimation(AnimationType.Attack);
            target.GetComponent<PlayerController>().onDamaged.Invoke(enemyData.damage);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Projectile"))
            {
                var projectile = collision.GetComponent<Projectile>();
                var damage = playerController._playerData.damage;
                GotDamaged(damage);
            }
        }

        public void GotDamaged(int damageAmount)
        {
            currentHealth -= damageAmount;
            print("Enemy got damaged remaining health:" + currentHealth);

            if (currentHealth < 0)
            {
                currentHealth = 0;
                Destroy(gameObject, 0.3f);
            }
        }
    }
}