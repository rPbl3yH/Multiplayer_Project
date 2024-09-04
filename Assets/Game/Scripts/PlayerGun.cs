using UnityEngine;

namespace Game.Core
{
    public class PlayerGun : Gun
    {
        [SerializeField] private Transform _firePoint;

        [SerializeField] private float _shootForce = 5f;
        [SerializeField] private float _shootDelay = .2f;
        [SerializeField] private int _damage = 1;
        
        private float _lastShootTime;
        
        public bool TryShoot(out ShootData shootData)
        {
            shootData = new ShootData();
            
            if(Time.time - _lastShootTime < _shootDelay) return false;
            
            _lastShootTime = Time.time;
            var spawnPosition = _firePoint.position;
            var velocity = _firePoint.forward * _shootForce;
            
            var bullet = Instantiate(BulletPrefab, spawnPosition, _firePoint.rotation);
            bullet.Construct(velocity, _damage);
            OnShoot?.Invoke();

            shootData.pX = spawnPosition.x;
            shootData.pY = spawnPosition.y;
            shootData.pZ = spawnPosition.z;

            shootData.dX = velocity.x;
            shootData.dY = velocity.y;
            shootData.dZ = velocity.z;
            return true;
        }
    }
}