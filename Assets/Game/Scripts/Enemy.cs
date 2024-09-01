using System;
using UnityEngine;

namespace Game.Core
{
    public class Enemy : Character
    {
        public Vector3 TargetPosition => _targetPosition;
        
        private Vector3 _targetPosition;
        private float _velocityMagnitude;

        private void Start()
        {
            _targetPosition = transform.position;
        }

        private void Update()
        {
            if (_velocityMagnitude > Mathf.Epsilon)
            {
                var distance = _velocityMagnitude * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, distance);
            }
            else
            {
                transform.position = _targetPosition;
            }
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }
        
        public void SetMovementData(in Vector3 position, in Vector3 velocity, in float interval)
        {
            _velocityMagnitude = velocity.magnitude;
            _targetPosition = position + velocity * interval;
            Velocity = velocity;
        }
    }
}