using System;
using System.Diagnostics;
using UnityEngine;

namespace Game.Core
{
    public class Gun : MonoBehaviour
    {
        public event Action OnShoot;
        
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _shootForce = 5f;

        [SerializeField] private float _shootDelay = .2f;
        private float _lastShootTime;
        
        public void Shoot()
        {
            if(Time.time - _lastShootTime < _shootDelay) return;
            
            _lastShootTime = Time.time;
            var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            bullet.Construct(_firePoint.forward, _shootForce);
            OnShoot?.Invoke();
        }
    }
}