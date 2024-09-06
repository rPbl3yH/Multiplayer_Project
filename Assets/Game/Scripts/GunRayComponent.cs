using System;
using UnityEngine;

namespace Game.Core
{
    public class GunRayComponent : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Transform _ray;
        [SerializeField] private Transform _pointer;
        [SerializeField] private float _size = .025f;
        [SerializeField] private float _maxDistance = 30f;
        [SerializeField] private float _minDistance = 5f;
        [SerializeField] private LayerMask _aimMask;

        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var hitDistance = _minDistance;
            var hitPosition = _firePoint.position + _firePoint.forward * _minDistance;
            
            if (Physics.Raycast(_firePoint.position, _firePoint.forward, out var hit, _maxDistance, _aimMask))
            {
                hitDistance = hit.distance;
                hitPosition = hit.point;
            }

            _ray.localScale = new Vector3(1f, 1f, hitDistance);
            _pointer.transform.position = hitPosition;

            var cameraDistance = Vector3.Distance(_camera.transform.position, hitPosition);
            _pointer.localScale = Vector3.one * (cameraDistance * _size);
        }
    }
}