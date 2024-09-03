using TMPro;
using UnityEngine;

namespace Modules.BaseUI
{
    public class TextBaseButton : BaseUIButton
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}