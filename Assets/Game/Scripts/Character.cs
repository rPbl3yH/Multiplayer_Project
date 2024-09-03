using UnityEngine;

namespace Game.Core
{
    public class Character : MonoBehaviour
    {
        [field: SerializeField] 
        public float Speed { get; protected set; } = 2;

        [field: SerializeField]
        public int MaxHealth { get; protected set; } = 10;
        
        public Vector3 Velocity { get; protected set; }
    }
}