using System;
using UnityEngine;

namespace _Scripts
{
    public class CharacterMovement : MonoBehaviour
    {
        public float speed = 10.0f;
        private CharacterAnimationController _animationController;

        private void Start()
        {
            _animationController = GetComponent<CharacterAnimationController>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            var movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
            transform.position += movement;

            if (horizontalInput != 0 || verticalInput != 0)
            {
                _animationController.PlayAnimation(AnimationType.Move);
            }
            else
            {
                _animationController.PlayAnimation(AnimationType.Idle);
            }
        }
    }

    public enum AnimationType
    {
        Idle,
        Attack,
        Move,
        Hurt,
        Death
    }
}