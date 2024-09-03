using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _lifeTime = 5f;
        
        [ShowInInspector, HideInEditorMode] private int _damage;

        public void Construct(in Vector3 velocity, int damage = 0)
        {
            _rigidbody.velocity = velocity;
            _damage = damage;

            StartCoroutine(LifeCoroutine());
        }

        private IEnumerator LifeCoroutine()
        {
            yield return new WaitForSecondsRealtime(_lifeTime);
            Destroy();
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyDamage(_damage);
            }
            Destroy();
        }
    }
}