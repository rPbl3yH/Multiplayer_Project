using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.BaseUI
{
    public class HudService : MonoBehaviour
    {
        public event Action<HudType> HudOpened;
        public event Action<HudType> HudClosed;

        [SerializeField] private List<HudView> _hudViews = new();
        [SerializeField] private bool _isLogWarning = true;
        
        private List<HudView> _openedHudViews = new();

        private readonly Dictionary<HudType, HudView> _menuTable = new();

        public void Awake()
        {
            RegisterAllView();
        }
        
        [Button]
        public void Show(HudType type)
        {
            if (type == HudType.None)
            {
                return;
            }
            
            if (!HudViewExist(type))
            {
                Debug.LogWarning($"You are trying to open a Menu {type} that has not been registered.");
                return;
            }

            HudView menu = GetView(type);
            menu.Show();
            _openedHudViews.Add(menu);

            HudOpened?.Invoke(menu.Type);
        }

        [Button]
        public void Hide(HudType hudType)
        {
            if (_openedHudViews.Count <= 0)
            {
                if(_isLogWarning)
                    Debug.LogWarning($"Hud {hudType} is not exist in opened huds");
                return;
            }

            var hudView = _openedHudViews.FirstOrDefault(view => view.Type == hudType);
            
            if (hudView != null)
            {
                hudView.Hide();
                _openedHudViews.Remove(hudView);
                HudClosed?.Invoke(hudView.Type);
            }
        }

        private void RegisterAllView()
        {
            foreach (var hudView in _hudViews)
            {
                RegisterHudView(hudView);

                // disable menu after register to hash table.
                hudView.Hide();
            }
            Debug.Log("Successfully registered all hudViews.");
        }

        private void RegisterHudView(HudView menu)
        {
            if (menu.Type == HudType.None)
            {
                Debug.LogWarning($"You are trying to register a {menu.Type} type menu that has not allowed.");
                return;
            }

            if (HudViewExist(menu.Type))
            {
                Debug.LogWarning($"You are trying to register a Menu {menu.Type} that has already been registered.");
                return;
            }

            _menuTable.Add(menu.Type, menu);
        }

        private HudView GetView(HudType type)
        {
            if (!HudViewExist(type)) return null;

            return _menuTable[type];
        }

        private bool HudViewExist(HudType type)
        {
            return _menuTable.ContainsKey(type);
        }

        public List<HudView> GetAll()
        {
            return _hudViews;
        }
    }
}