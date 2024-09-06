using System;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

namespace Game.Core
{
    public class Player : Character
    {
        public PlayerController PlayerController { get; private set; }
        
        [SerializeField] private Health _health;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _cameraPoint;

        [SerializeField] private float _minHeadAngle = -90f;
        [SerializeField] private float _maxHeadAngle = 90f;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 50f;
        [SerializeField] private CheckFly _checkFly;
        [SerializeField] private float _jumpDelay = .2f;

        [SerializeField] private PlayerGun _gun;
        
        private Vector3 _moveDirection;
        private float _yRotate;
        private float _xRotate;
        private float _jumpTime;

        public void Construct(PlayerController playerController)
        {
            PlayerController = playerController;
        }

        private void Start()
        {
            var cameraTransform = Camera.main.transform;
            cameraTransform.SetParent(_cameraPoint);
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localRotation = Quaternion.identity;
            
            _health.Construct(MaxHealth);
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
            velocity *= Speed;
            
            velocity.y = _rigidbody.velocity.y;
            Velocity = velocity;
            
            _rigidbody.velocity = Velocity;
        }

        private void Rotate()
        {
            _rigidbody.angularVelocity = new Vector3(0f, _yRotate, 0f);
            _yRotate = 0f;
        }

        public void Jump()
        {
            if (_checkFly.IsFly) return;

            if (Time.time - _jumpTime < _jumpDelay) return;

            _jumpTime = Time.time;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float eulerX, out float eulerY)
        {
            position = transform.position;
            velocity = _rigidbody.velocity;

            eulerX = _head.localEulerAngles.x;
            eulerY = transform.eulerAngles.y;
        }

        public bool TryShoot(out ShootData shootData)
        {
            return _gun.TryShoot(out shootData);
        }

        public void OnChange(List<DataChange> changes)
        {
            foreach (var dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "loss":
                        var playerLose = (ushort)dataChange.Value;
                        PlayerUIManager.Instance.ScoreView.SetPlayerScore(playerLose);
                        break;
                    case "hp":
                        var hp = (short)dataChange.Value;
                        _health.SetHealth(hp);
                        break;
                    default:
                        // Debug.Log($"No handler for data {dataChange.Field}");
                        break;
                }
            }
        }

        public void ResetVelocity()
        {
            Velocity = Vector3.zero;
        }
    }
}
