using Modules.BaseUI;
using UnityEngine;

namespace Game.Core
{
    public class HealthObserver : MonoBehaviour
    {
        [SerializeField] private BaseProgressBar _baseProgressBar;
        [SerializeField] private Health _health;

        private void OnEnable()
        {
            _health.OnHealthChanged += OnHealthChanged;
            _health.OnMaxHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= OnHealthChanged;
            _health.OnMaxHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int health)
        {
            var progress = (float)_health.Current / _health.MaxHealth;
            _baseProgressBar.SetProgress(progress);
        }
    }
}