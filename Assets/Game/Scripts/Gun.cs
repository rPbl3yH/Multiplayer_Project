using System;
using System.Diagnostics;
using UnityEngine;

namespace Game.Core
{
    public abstract class Gun : MonoBehaviour
    {
        public Action OnShoot;
        
        [SerializeField] protected Bullet BulletPrefab;
    }
}