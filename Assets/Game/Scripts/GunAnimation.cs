using UnityEngine;

namespace Game.Core
{
    public class GunAnimation : MonoBehaviour
    {
        [SerializeField] private Gun _gun;
        [SerializeField] private Animator _animator;
        
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        private void OnEnable()
        {
            _gun.OnShoot += OnGunShoot;
        }

        private void OnDisable()
        {
            _gun.OnShoot -= OnGunShoot;
        }

        private void OnGunShoot()
        {
            _animator.SetTrigger(Shoot);
        }
    }
}