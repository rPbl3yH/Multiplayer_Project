using UnityEngine;

namespace Game.Core
{
    public class EnemyGun : Gun
    {
        public void Shoot(in Vector3 position, in Vector3 velocity)
        {
            var bullet = Instantiate(BulletPrefab, position, Quaternion.identity);
            bullet.Construct(velocity);
            OnShoot?.Invoke();
        }
    }
}