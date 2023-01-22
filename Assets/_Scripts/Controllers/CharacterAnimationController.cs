using System;
using UnityEngine;

namespace _Scripts.Controllers
{
    public class CharacterAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private AnimationType _currentAnimationType;

        #region AnimationNames

        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int IsMoving = Animator.StringToHash("IsWalking");
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        #endregion

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAnimation(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Idle:
                    if (_currentAnimationType == AnimationType.Idle) return;
                    _currentAnimationType = AnimationType.Idle;
                    _animator.SetTrigger(IsIdle);
                    break;
                case AnimationType.Attack:
                    _currentAnimationType = AnimationType.Attack;
                    _animator.SetTrigger(IsAttacking);
                    break;
                case AnimationType.Move:
                    if (_currentAnimationType == AnimationType.Move) return;
                    _currentAnimationType = AnimationType.Move;
                    _animator.SetTrigger(IsMoving);
                    break;
                case AnimationType.Hurt:
                    _currentAnimationType = AnimationType.Hurt;
                    _animator.SetTrigger(IsHurt);
                    break;
                case AnimationType.Death:
                    _currentAnimationType = AnimationType.Death;
                    _animator.SetTrigger(IsDead);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
            }
        }
    }
}