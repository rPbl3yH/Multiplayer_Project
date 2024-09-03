using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BaseUI
{
    public abstract class BaseProgressBar : BaseUIView
    {
        [field: SerializeField] public Image ProgressImage { get; protected set; }

#if ODIN_INSPECTOR
        [Button]
#endif
        public abstract void SetProgress(float progress);
    }
}