using Reflectis.CreatorKit.Worlds.Core.Interaction;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis: Expose Generic Interactable")]
    [UnitSurtitle("Expose")]
    [UnitShortTitle("Generic Interactable")]
    [UnitCategory("Reflectis\\Expose")]
    public class ExposeVisualScriptingInteractableUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Interactable { get; private set; }

        [DoNotSerialize]
        public ValueOutput GameObjectReference { get; private set; }

        [DoNotSerialize]
        public ValueOutput InteractionState { get; private set; }

        [DoNotSerialize]
        public ValueOutput InteractionColliders { get; private set; }

        protected override void Definition()
        {
            Interactable = ValueInput<IVisualScriptingInteractable>(nameof(Interactable), null).NullMeansSelf();

            GameObjectReference = ValueOutput(nameof(GameObjectReference), (flow) => flow.GetValue<IVisualScriptingInteractable>(Interactable).InteractableRef.GameObjectRef);

            InteractionState = ValueOutput(nameof(InteractionState), (flow) => flow.GetValue<IVisualScriptingInteractable>(Interactable).InteractableRef.InteractionState);

            InteractionColliders = ValueOutput(nameof(InteractionColliders), (flow) => flow.GetValue<IVisualScriptingInteractable>(Interactable).InteractableRef.InteractionColliders);
        }


    }
}
