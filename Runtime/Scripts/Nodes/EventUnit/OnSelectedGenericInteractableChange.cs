using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Visual Scripting Interactable: On Selected Change")]
    [UnitSurtitle("VisualScriptingInteractable")]
    [UnitShortTitle("On Selected Change")]
    [UnitCategory("Events\\Reflectis")]
    public class OnSelectedVisualScriptingInteractableChange : UnityEventUnit<VisualScriptingInteractable, IVisualScriptingInteractable>
    {
        [DoNotSerialize]
        public ValueOutput VisualScriptingInteractable { get; private set; }
        protected override bool register => true;

        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            VisualScriptingInteractable = ValueOutput<IVisualScriptingInteractable>(nameof(VisualScriptingInteractable));
        }

        protected override void AssignArguments(Flow flow, IVisualScriptingInteractable data)
        {
            flow.SetValue(VisualScriptingInteractable, data);
        }

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook("VisualScriptingInteractable" + this.ToString().Split("EventUnit")[0]);
        }

        protected override UnityEvent<IVisualScriptingInteractable> GetEvent(GraphReference reference)
        {
            return SM.GetSystem<IVisualScriptingInteractionSystem>().OnSelectedInteractableChange;
        }

        protected override VisualScriptingInteractable GetArguments(GraphReference reference, IVisualScriptingInteractable eventData)
        {
            return eventData as VisualScriptingInteractable;
        }
    }
}
