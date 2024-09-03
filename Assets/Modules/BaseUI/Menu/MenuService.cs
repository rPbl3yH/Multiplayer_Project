using System.Collections.Generic;
using UnityEngine;

namespace Modules.BaseUI
{
    public class MenuService : MonoBehaviour
    {
        public MenuType GetCurrentMenu => _currentMenu;
        
        [SerializeField] private List<MenuView> _menuViews = new();
        [SerializeField] private bool _isLogWarning = true;
        
        private Stack<MenuView> _menuStack = new();
        private MenuType _currentMenu;
        
        private readonly Dictionary<MenuType, MenuView> _menuTable = new();

        private void Awake()
        {
            RegisterAllMenus();
        }

        public void SwitchMenu(MenuType type)
        {
            HideMenu();    
            ShowMenu(type);
        }

        public void ShowMenu(MenuType type)
        {
            if (type == MenuType.None)
            {
                return;
            }
            
            if (!MenuExist(type))
            {
                Debug.LogWarning($"You are trying to open a Menu {type} that has not been registered.");
                return;
            }

            MenuView menu = GetMenu(type);
            menu.Show();
            _menuStack.Push(menu);

            _currentMenu = menu.Type;
        }

        public void HideMenu()
        {
            if (_menuStack.Count <= 0)
            {
                if(_isLogWarning)
                    Debug.LogWarning("MenuController CloseMenu ERROR: No menus in stack!");
                return;
            }
            
            MenuView lastMenuStack = _menuStack.Pop();
            lastMenuStack.Hide();

            if (_menuStack.Count > 0)
            {
                _currentMenu = _menuStack.Peek().Type;
            }
            else
            {
                _currentMenu = MenuType.None;
            }
        }

        private void RegisterAllMenus()
        {
            foreach (var menu in _menuViews)
            {
                RegisterMenu(menu);

                // disable menu after register to hash table.
                menu.Hide();
            }
            Debug.Log("Successfully registered all menus.");
        }

        private void RegisterMenu(MenuView menu)
        {
            if (menu.Type == MenuType.None)
            {
                Debug.LogWarning($"You are trying to register a {menu.Type} type menu that has not allowed.");
                return;
            }

            if (MenuExist(menu.Type))
            {
                Debug.LogWarning($"You are trying to register a Menu {menu.Type} that has already been registered.");
                return;
            }

            _menuTable.Add(menu.Type, menu);
        }

        private MenuView GetMenu(MenuType type)
        {
            if (!MenuExist(type)) return null;

            return _menuTable[type];
        }

        private bool MenuExist(MenuType type)
        {
            return _menuTable.ContainsKey(type);
        }
    }
}