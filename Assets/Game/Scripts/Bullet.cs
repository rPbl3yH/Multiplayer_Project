using System;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _lifeTime = 5f;

        public void Construct(in Vector3 velocity)
        {
            _rigidbody.velocity = velocity;

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
            Debug.Log($"On collision = {other.collider.name}");
            Destroy();
        }
    }
}