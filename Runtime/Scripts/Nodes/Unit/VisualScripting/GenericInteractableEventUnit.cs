using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.SDK.Core.VisualScripting;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    public abstract class VisualScriptingInteractableEventUnit : AwaitableEventUnit<IVisualScriptingInteractable>
    {
        [DoNotSerialize]
        public ValueOutput Interactable { get; private set; }

        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            Interactable = ValueOutput<IVisualScriptingInteractable>(nameof(Interactable));
        }

        // Setting the value on our port.
        protected override void AssignArguments(Flow flow, IVisualScriptingInteractable data)
        {
            flow.SetValue(Interactable, data);
        }

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook("VisualScriptingInteractable" + this.ToString().Split("Unit")[0]);
        }
    }
}
