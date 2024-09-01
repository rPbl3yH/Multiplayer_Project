using System;
using UnityEngine;

namespace Game.Core
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private CheckFly _checkFly;
        [SerializeField] private Animator _animator;
        [SerializeField] private Character _character;
        
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Update()
        {
            var localVelocity = transform.InverseTransformVector(_character.Velocity);
            var speed = localVelocity.magnitude / _character.Speed;
            var sign = Mathf.Sign(localVelocity.z);
            
            _animator.SetFloat(Speed, speed * sign);
            _animator.SetBool(Grounded, !_checkFly.IsFly);
        }
    }
}
