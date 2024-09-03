using UnityEngine;

namespace Game.Core
{
    public class PlayerGun : Gun
    {
        [SerializeField] private Transform _firePoint;

        [SerializeField] private float _shootForce = 5f;
        [SerializeField] private float _shootDelay = .2f;
        private float _lastShootTime;
        
        public bool TryShoot(out ShootInfo shootInfo)
        {
            shootInfo = new ShootInfo();
            
            if(Time.time - _lastShootTime < _shootDelay) return false;
            
            _lastShootTime = Time.time;
            var spawnPosition = _firePoint.position;
            var velocity = _firePoint.forward * _shootForce;
            
            var bullet = Instantiate(BulletPrefab, spawnPosition, _firePoint.rotation);
            bullet.Construct(velocity);
            OnShoot?.Invoke();

            shootInfo.pX = spawnPosition.x;
            shootInfo.pY = spawnPosition.y;
            shootInfo.pZ = spawnPosition.z;

            shootInfo.dX = velocity.x;
            shootInfo.dY = velocity.y;
            shootInfo.dZ = velocity.z;
            return true;
        }
    }
}