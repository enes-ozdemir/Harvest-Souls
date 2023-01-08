using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class Soul : MonoBehaviour, ICollectable
    {
        private int _amount;

        public void Setup(int amount)
        {
            _amount = amount;
        }

        public int Collect(Transform transform)
        {
            gameObject.transform.DOMove(transform.position, 0.1f);
            Destroy(gameObject, 0.3f);
            return _amount;
        }
    }
}