using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.SDK.Core.SystemFramework;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Generic Interactable: On Selected Change")]
    [UnitSurtitle("GenericInteractable")]
    [UnitShortTitle("On Selected Change")]
    [UnitCategory("Events\\Reflectis")]
    public class OnSelectedGenericInteractableChange : EventUnit<IVisualScriptingInteractable>
    {
        [DoNotSerialize]
        public ValueOutput GenericInteractable { get; private set; }
        protected override bool register => true;

        protected GraphReference graphReference;

        protected IVisualScriptingInteractable interactableReference;

        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            GenericInteractable = ValueOutput<IVisualScriptingInteractable>(nameof(IVisualScriptingInteractable));
        }

        protected override void AssignArguments(Flow flow, IVisualScriptingInteractable data)
        {
            flow.SetValue(GenericInteractable, interactableReference);
        }

        public override EventHook GetHook(GraphReference reference)
        {
            graphReference = reference;

            return new EventHook("GenericInteractable" + this.ToString().Split("EventUnit")[0]);
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
            Trigger(graphReference, interactableReference);
        }
    }
}
