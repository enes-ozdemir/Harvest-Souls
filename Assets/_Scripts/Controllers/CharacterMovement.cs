using UnityEngine;

namespace _Scripts.Controllers
{
    public class CharacterMovement : MonoBehaviour
    {
        private bool _facingRight = true;
        private CharacterAnimationController _animationController;
        
        public float speed = 10.0f;

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
            if (inputX > 0 && _facingRight)
            {
                Flip();
                _facingRight = false;
            }
            else if (inputX < 0 && !_facingRight)
            {
                Flip();
                _facingRight = true;
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