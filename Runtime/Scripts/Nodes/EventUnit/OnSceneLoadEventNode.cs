using Reflectis.SDK.Core.VisualScripting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Scene: On Load")]
    [UnitSurtitle("Scene")]
    [UnitShortTitle("On Load")]
    [UnitCategory("Events\\Reflectis")]
    public class OnSceneLoadEventNode : AwaitableEventUnit<string>
    {
        public static string eventName = "OnSceneLoad";

        public static Dictionary<GraphReference, List<OnSceneLoadEventNode>> instances = new Dictionary<GraphReference, List<OnSceneLoadEventNode>>();

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            if (instances.TryGetValue(reference, out var value))
            {
                if (!value.Contains(this))
                {
                    value.Add(this);
                }
            }
            else
            {
                List<OnSceneLoadEventNode> variableList = new List<OnSceneLoadEventNode>
                {
                    this
                };

                instances.Add(reference, variableList);
            }

            return new EventHook(eventName);
        }

        public override void Uninstantiate(GraphReference instance)
        {
            base.Uninstantiate(instance);
            instances.Remove(instance);
        }

        public static async Task TriggerAllNodes()
        {
            List<Task> providerEnterTask = new List<Task>();

            foreach (GraphReference reference in instances.Keys)
            {
                foreach (var node in instances[reference])
                {
                    providerEnterTask.Add(node.AwaitableTrigger(reference, ""));
                }
            }

            await Task.WhenAll(providerEnterTask);

        }
    }
}

