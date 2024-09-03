using TMPro;
using UnityEngine;

namespace Modules.BaseUI.TextView
{
    public class TitleBaseUIView : TextBaseUIView
    {
        [SerializeField] private TMP_Text _titleText;

        public virtual void SetTitleText(string text)
        {
            _titleText.SetText(text);
        }
    }
}