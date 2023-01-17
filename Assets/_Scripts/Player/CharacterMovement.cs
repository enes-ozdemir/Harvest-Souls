using System;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts
{
    public class CharacterMovement : MonoBehaviour
    {
        public float speed = 10.0f;
        private CharacterAnimationController _animationController;
        public static bool facingRight = true;

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
            FlipPlayer(horizontalInput);
            PlayAnimation(horizontalInput, verticalInput);
        }

        private void PlayAnimation(float horizontalInput, float verticalInput)
        {
            if (horizontalInput != 0 || verticalInput != 0)
                _animationController.PlayAnimation(AnimationType.Move);
            else
                _animationController.PlayAnimation(AnimationType.Idle);
        }

        private void FlipPlayer(float inputX)
        {
            if (inputX > 0 && facingRight)
            {
                Flip();
                facingRight = false;
            }
            else if (inputX < 0 && !facingRight)
            {
                Flip();
                facingRight = true;
            }
        }

        private void Flip()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            var aimHelper = transform.GetChild(0);
            var aimHelperScale = aimHelper.localScale;
            aimHelperScale.x *= -1;
            aimHelper.localScale = scale;
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