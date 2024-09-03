using UnityEngine.Events;
using UnityEngine.UI;

namespace Modules.BaseUI
{
    public static class ButtonExtensions
    {
        public static void Subscribe(this Button button, UnityAction unityAction)
        {
            button.onClick.AddListener(unityAction);
        }

        public static void Unsubscribe(this Button button, UnityAction unityAction)
        {
            button.onClick.RemoveListener(unityAction);
        }

        public static void RemoveAllSubscribers(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}