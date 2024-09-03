using UnityEngine;

namespace Modules.BaseUI
{
    public class HudView : BaseUIView
    {
        public HudType Type => _type;
        
        [SerializeField] private HudType _type;
    }
}