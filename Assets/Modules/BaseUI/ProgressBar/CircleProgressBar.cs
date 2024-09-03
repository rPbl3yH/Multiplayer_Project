using TMPro;
using UnityEngine;

namespace Modules.BaseUI
{
    public class CircleProgressBar : BaseProgressBar
    {
        [SerializeField] private TMP_Text _progressText;

        [SerializeField] private float _fillDuration = 1f;
        
        // private Tween _tween;
        
        public override void SetProgress(float progress)
        {
            // print($"Progress = {progress}");
            // _tween?.Kill();
            // _tween = _progressImage.DOFillAmount(progress, _fillDuration)
            // .SetLink(gameObject);
        }

        public void SetText(string text)
        {
            if (_progressText != null)
            {
                _progressText.text = $"{text}";
            }
        }
    }
}