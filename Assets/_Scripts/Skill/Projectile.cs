using System;
using _Scripts.Util;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Skill
{
    public class Projectile : MonoBehaviour
    {
        private Vector3 _shootDir;
        public float moveSpeed;
        public int projectileDamage;
        public GameObject explosivePrefab;
        private bool _isDestroyed;

        public void Setup(Vector3 shootDirection, int damage)
        {
            projectileDamage = damage;
            _shootDir = shootDirection;
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(_shootDir));
            DestroyProjectile(5f);
        }

        private async void DestroyProjectile(float seconds)
        {
            if (_isDestroyed) return;
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            EndEffect();
        }

        private void EndEffect()
        {
            if (_isDestroyed) return;
            ObjectPooler.Instance.SpawnFromPool("ProjectileExplosive", transform.position,
                Quaternion.identity);
            if (gameObject == null) return;
            Destroy(gameObject);
            explosivePrefab.gameObject.SetActive(false);
            _isDestroyed = true;
        }

        public void DestroyIt()
        {
            EndEffect();
        }

        private void Update()
        {
            transform.position += _shootDir * moveSpeed * Time.deltaTime;
        }

        private float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Debug.Log("Entered");
            // var enemy = collision.GetComponent<Enemy>();
            // if (enemy != null)
            // {
            //     enemy.GotDamaged(10);
            //    Destroy(gameObject);
            // }
        }
    }
}