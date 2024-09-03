using UnityEngine;

namespace Modules.BaseUI
{
    public class MenuView : BaseUIView
    {
        public MenuType Type => _type;
        
        [SerializeField] private MenuType _type;
    }
    
    public enum MenuType
    {
        None,
        Main,
        Lose,
        Win,
        GameMenu,
        AdsLose,
    }
}