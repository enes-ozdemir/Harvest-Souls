using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts
{
    public class Projectile : MonoBehaviour
    {
        private Vector3 _shootDir;
        public float moveSpeed;
        public int projectileDamage;
        public GameObject explosivePrefab;
        private bool isDestroyed;

        public void Setup(Vector3 shootDirection, int damage)
        {
            projectileDamage = damage;
            _shootDir = shootDirection;
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(_shootDir));
            DestroyProjectile(5f);
        }

        private async void DestroyProjectile(float seconds)
        {
            if (isDestroyed) return;
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            if (gameObject == null) return;
            EndEffect();
        }

        private void EndEffect()
        {
            if (isDestroyed) return;
            Instantiate(explosivePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            explosivePrefab.gameObject.SetActive(false);
            isDestroyed = true;
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