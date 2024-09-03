using System;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BaseUI
{
    public class ProgressBar : BaseProgressBar
    {
        [SerializeField] private Slider _slider;

        public void SetValue(float value)
        {
        }

        public override void Show()
        {
            _slider.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            _slider.gameObject.SetActive(false);
        }

        public override void SetProgress(float progress)
        {
            _slider.value = progress;
        }
    }
}