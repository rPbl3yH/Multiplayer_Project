using UnityEngine;

namespace Game.Core
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public void Construct(Vector3 direction, float speed)
        {
            _rigidbody.velocity = direction * speed;
        }
    }
}