using _Scripts.Player;
using _Scripts.Skill;
using _Scripts.Util;
using UnityEngine;

namespace _Scripts
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField] private PlayerAimController playerAimWeapon;
        [SerializeField] private Transform projectile;
        private GameObject _projectileTransform;
        [SerializeField] private PlayerController playerController;

        public void Start()
        {
            playerAimWeapon.OnShoot += SpawnParticle;
            playerAimWeapon.attackSpeed = playerController.GetAttackSpeed();
        }


        private void SpawnParticle(PlayerAimController.OnShootEventArgs args)
        {
            print("Spawned");
            _projectileTransform =ObjectPooler.Instance.SpawnFromPool("Projectile", args.gunEndPointPosition,
                Quaternion.identity);
            var shootDir = (args.shootPosition - args.gunEndPointPosition).normalized;
            _projectileTransform.GetComponent<Projectile>().Setup(shootDir, playerController.GetDamage());
        }

        public void OnDestroy()
        {
            playerAimWeapon.OnShoot -= SpawnParticle;
        }
    }
}