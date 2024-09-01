using UnityEngine;

namespace Game.Core
{
    public class Character : MonoBehaviour
    {
        [field: SerializeField]
        public float Speed { get; protected set; }
        
        public Vector3 Velocity { get; protected set; }
    }
}