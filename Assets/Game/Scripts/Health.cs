using System;
using UnityEngine;

namespace Game.Core
{
    public class Health : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;
        public event Action<int> OnMaxHealthChanged;
        
        public int Current => _current;
        public int MaxHealth => _maxHealth;
        
        private int _maxHealth;
        private int _current;

        public void Construct(int maxHealth)
        {
            _maxHealth = maxHealth;
            _current = _maxHealth;
            OnMaxHealthChanged?.Invoke(_maxHealth);
        }

        public void SetHealth(int health)
        {
            _current = health;
            OnHealthChanged?.Invoke(_current);
            
            if (_current == 0)
            {
                Debug.Log($"Object {gameObject.name} is dead");
            }
        }

        public void TakeDamage(int damage)
        {
            var appliedDamage = Mathf.Min(_current, damage);
            var hp = _current - appliedDamage;
            SetHealth(hp);
        }
    }
}