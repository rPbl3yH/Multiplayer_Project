using UnityEngine;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 3f;
        
        private Vector3 _moveDirection;

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            var velocity = _moveDirection.z * transform.forward + _moveDirection.x * transform.right;
            velocity.Normalize();
            velocity *= _speed;
            _rigidbody.velocity = velocity;
            
            // _rigidbody.velocity = _moveDirection * _speed;
            // transform.position += _moveDirection * _speed * Time.deltaTime;
        }
        
        public void SetupMoveDirection(Vector3 moveDirection)
        {
            _moveDirection = moveDirection;
        }

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
        {
            position = transform.position;
            velocity = _rigidbody.velocity;
        }
    }
}
