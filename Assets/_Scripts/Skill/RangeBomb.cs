using UnityEngine;

namespace _Scripts.Skill
{
    public class RangeBomb : MonoBehaviour
    {
        private float _range;
        private int _damage;

        private void Start()
        {
            transform.localScale = new Vector3(_range, _range, _range);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Enemy")) return;

            var enemy = col.GetComponent<Enemy>();
            enemy.GotDamaged(_damage);
        }
    }
}