using Reflectis.SDK.Core.VisualScripting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Scene: On Setup")]
    [UnitSurtitle("Scene")]
    [UnitShortTitle("On Setup")]
    [UnitCategory("Events\\Reflectis")]
    public class OnSceneSetupEventNode : AwaitableEventUnit<string>
    {
        public static string eventName = "OnSceneSetup";

        public static Dictionary<GraphReference, List<OnSceneSetupEventNode>> instances = new Dictionary<GraphReference, List<OnSceneSetupEventNode>>();

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
                List<OnSceneSetupEventNode> variableList = new List<OnSceneSetupEventNode>
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

