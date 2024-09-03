using UnityEngine;

namespace Modules.BaseUI
{
    public class SizeProgressBar : BaseProgressBar
    {
        [SerializeField] private float _defaultDeltaX;

        public override void SetProgress(float progress)
        {
            var deltaY = ProgressImage.rectTransform.sizeDelta.y;
            ProgressImage.rectTransform.sizeDelta = new Vector2(_defaultDeltaX * progress, deltaY);
        }

        private void OnValidate()
        {
            _defaultDeltaX = ProgressImage.rectTransform.sizeDelta.x;
        }
    }
}