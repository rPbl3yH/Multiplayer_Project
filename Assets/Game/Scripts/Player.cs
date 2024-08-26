using System;
using UnityEngine;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _cameraPoint;

        [SerializeField] private float _minHeadAngle = -90f;
        [SerializeField] private float _maxHeadAngle = 90f;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 50f;
        
        private Vector3 _moveDirection;
        private float _yRotate;
        private float _xRotate;

        private bool _isGrounded;

        private void Start()
        {
            var cameraTransform = Camera.main.transform;
            cameraTransform.SetParent(_cameraPoint);
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localRotation = Quaternion.identity;
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
        }

        public void Rotate(float xAngle)
        {
            _xRotate = Mathf.Clamp(_xRotate + xAngle, _minHeadAngle, _maxHeadAngle);
            _head.localEulerAngles = new Vector3(_xRotate, 0f, 0f);
        }

        public void SetInput(Vector3 moveDirection, float yRotate)
        {
            _moveDirection = moveDirection;
            _yRotate += yRotate;
        }

        private void Move()
        {
            var velocity = _moveDirection.z * transform.forward + _moveDirection.x * transform.right;
            velocity.Normalize();
            velocity *= _speed;
            
            velocity.y = _rigidbody.velocity.y;
            
            _rigidbody.velocity = velocity;
            
            // _rigidbody.velocity = _moveDirection * _speed;
            // transform.position += _moveDirection * _speed * Time.deltaTime;
        }

        private void Rotate()
        {
            _rigidbody.angularVelocity = new Vector3(0f, _yRotate, 0f);
            _yRotate = 0f;
        }

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
        {
            position = transform.position;
            velocity = _rigidbody.velocity;
        }

        public void Jump()
        {
            if (!_isGrounded)
            {
                return;
            }
            
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }

        private void OnCollisionEnter(Collision other)
        {
            foreach (ContactPoint contactPoint in other.contacts) 
            {
                if (contactPoint.normal.y > 0.45f)
                {
                    _isGrounded = true;
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            _isGrounded = false;
        }
    }
}
