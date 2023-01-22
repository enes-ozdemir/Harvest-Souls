using System;
using _Scripts.Controllers;
using _Scripts.Managers;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts
{
    public class PlayerAimController : MonoBehaviour
    {
        [SerializeField] private Transform staffEndPointTransform;
        [SerializeField] private Transform aimHelper;

        private bool _isInBattle;
        private Camera _camera;
        private float _attackTimer;
        public float attackSpeed = 7f;

        public Action<OnShootEventArgs> OnShoot;

        public class OnShootEventArgs : EventArgs
        {
            public Vector3 gunEndPointPosition;
            public Vector3 shootPosition;
        }

        private CharacterAnimationController _animationController;

        private void Awake()
        {
            BattleManager.onBattleStarted += SetBattleStarted;
            BattleManager.onBattleEnded += SetBattleEnded;
            _animationController = GetComponent<CharacterAnimationController>();
            
            _camera = Camera.main;
        }

        private void SetBattleStarted() => _isInBattle = true;
        private void SetBattleEnded() => _isInBattle = false;

        private void Update()
        {
            if (!_isInBattle) return;

            HandleWeaponDirection();
            HandleProjectile();
        }

        private void HandleWeaponDirection()
        {
            var mousePosition = GetMouseWorldPosition();
            var aimDirection = (mousePosition - transform.position).normalized;
            var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            aimHelper.eulerAngles = new Vector3(0, 0, angle + 180);
        }


        private Vector3 GetMouseWorldPosition()
        {
            var vec = _camera.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0f;
            return vec;
        }

        private void HandleProjectile()
        {
            if (Input.GetMouseButton(0))
            {
                _animationController.PlayAnimation(AnimationType.Attack);
                _attackTimer += Time.deltaTime;

                if (_attackTimer >= 1 / attackSpeed)
                {
                    // Reset the attack timer
                    _attackTimer = 0f;

                    var mousePosition = GetMouseWorldPosition();

                    OnShoot?.Invoke(new OnShootEventArgs
                    {
                        gunEndPointPosition = staffEndPointTransform.position,
                        shootPosition = mousePosition,
                    });
                }
            }
            else _attackTimer = 0f;
        }
    }
}