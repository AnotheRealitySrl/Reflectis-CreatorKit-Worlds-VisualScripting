using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.SDK.Core.SystemFramework;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Visual Scripting Interactable: On Selected Change")]
    [UnitSurtitle("VisualScriptingInteractable")]
    [UnitShortTitle("On Selected Change")]
    [UnitCategory("Events\\Reflectis")]
    public class OnSelectedVisualScriptingInteractableChange : EventUnit<VisualScriptingInteractable>
    {
        [DoNotSerialize]
        public ValueOutput VisualScriptingInteractable { get; private set; }
        protected override bool register => true;

        protected GraphReference graphReference;

        protected IVisualScriptingInteractable interactableReference;

        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            VisualScriptingInteractable = ValueOutput<IVisualScriptingInteractable>(nameof(IVisualScriptingInteractable));
        }

        protected override void AssignArguments(Flow flow, VisualScriptingInteractable data)
        {
            flow.SetValue(VisualScriptingInteractable, interactableReference);
        }

        public override EventHook GetHook(GraphReference reference)
        {
            graphReference = reference;

            return new EventHook("VisualScriptingInteractable" + this.ToString().Split("EventUnit")[0]);
        }

        public override void Instantiate(GraphReference instance)
        {
            base.Instantiate(instance);

            SM.GetSystem<IVisualScriptingInteractionSystem>().OnSelectedInteractableChange.AddListener(OnSelectedChange);
        }

        public override void Uninstantiate(GraphReference instance)
        {
            base.Uninstantiate(instance);

            SM.GetSystem<IVisualScriptingInteractionSystem>().OnSelectedInteractableChange.RemoveListener(OnSelectedChange);
        }

        private void OnSelectedChange(IVisualScriptingInteractable newSelection)
        {
            interactableReference = newSelection;
            Trigger(graphReference, interactableReference as VisualScriptingInteractable);
        }
    }
}
