using UnityEngine;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        
        private Vector3 _moveDirection;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += _moveDirection * _speed * Time.deltaTime;
        }
        
        public void SetupMoveDirection(Vector3 moveDirection)
        {
            _moveDirection = moveDirection;
        }

        public Vector3 GetMoveInfo()
        {
            return transform.position;
        }
    }
}
