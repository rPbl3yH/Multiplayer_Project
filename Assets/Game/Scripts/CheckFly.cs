using System;
using UnityEngine;

namespace Game.Core
{
    public class CheckFly : MonoBehaviour
    {
        public bool IsFly;

        [SerializeField] private float _radius = .3f;
        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private float _coyoteDelay = .15f;
        private float _coyoteTime;

        private void FixedUpdate()
        {
            if (Physics.CheckSphere(transform.position, _radius, _layerMask))
            {
                IsFly = false;
                _coyoteTime = 0f;
            }
            else
            {
                _coyoteTime += Time.deltaTime;
                
                if (_coyoteTime > _coyoteDelay)
                {
                    IsFly = true;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
#endif
    }
}
