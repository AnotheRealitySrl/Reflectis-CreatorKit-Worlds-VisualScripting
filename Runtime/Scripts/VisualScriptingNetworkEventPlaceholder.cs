using Virtuademy.CreatorKit.Worlds.Placeholders;

using UnityEngine;
using UnityEngine.Events;

namespace Virtuademy.CreatorKit.Worlds.VisualScripting
{
    public class VisualScriptingNetworkEventPlaceholder : SceneComponentPlaceholderNetwork
    {
        [HideInInspector]
        public UnityEvent<string> action = new UnityEvent<string>();

        public void ActionInvoke(string eventName)
        {
            action?.Invoke(eventName);
        }
    }
}