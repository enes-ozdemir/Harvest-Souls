using UnityEngine;

namespace _Scripts
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField] private PlayerAimController playerAimWeapon;
        [SerializeField] private Transform projectile;
        private Transform _projectileTransform;

        public void Start()
        {
            playerAimWeapon.OnShoot += SpawnParticle;
        }


        private void SpawnParticle(PlayerAimController.OnShootEventArgs args)
        {
            print("Spawned");
            _projectileTransform = Instantiate(projectile, args.gunEndPointPosition, Quaternion.identity);
            var shootDir = (args.shootPosition - args.gunEndPointPosition).normalized;
            _projectileTransform.GetComponent<Projectile>().Setup(shootDir);
        }

        public void OnDestroy()
        {
            playerAimWeapon.OnShoot -= SpawnParticle;
        }
    }
}