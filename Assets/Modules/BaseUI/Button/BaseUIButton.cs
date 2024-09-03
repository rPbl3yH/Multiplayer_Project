using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BaseUI
{
    public class BaseUIButton : BaseUIView
    {
        public event Action Clicked;
        public event Action UserClicked;

        [ShowInInspector, ReadOnly]
        private bool _isInteractable = true;
        
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.Subscribe(OnClicked);
        }

        private void OnDisable()
        {
            _button.Unsubscribe(OnClicked);
        }

        public void Deactivate()
        {
            _isInteractable = false;
        }
        
        public void Activate()
        {
            _isInteractable = true;
        }

        private void OnClicked()
        {
            UserClicked?.Invoke();
            
            if (_isInteractable)
            {
                Clicked?.Invoke();
            }
        }
    }
}