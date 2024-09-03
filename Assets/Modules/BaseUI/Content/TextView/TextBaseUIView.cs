using TMPro;
using UnityEngine;

namespace Modules.BaseUI.TextView
{
    public class TextBaseUIView : BaseUIView
    {
        [SerializeField] private TMP_Text _text;

        public virtual void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}