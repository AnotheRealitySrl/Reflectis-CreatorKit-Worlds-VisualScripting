using Reflectis.SDK.Core.Interaction;

using Unity.VisualScripting;

using static Reflectis.SDK.Core.Interaction.Manipulable;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis: Expose Manipulable")]
    [UnitSurtitle("Expose")]
    [UnitShortTitle("Manipulable")]
    [UnitCategory("Reflectis\\Expose")]
    public class ExposeManipulable : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Manipulable { get; private set; }

        [DoNotSerialize]
        public ValueOutput GameObjectReference { get; private set; }

        [DoNotSerialize]
        public ValueOutput InteractionState { get; private set; }

        [DoNotSerialize]
        public ValueOutput InteractionColliders { get; private set; }

        [DoNotSerialize]
        public ValueOutput IsManipulated { get; private set; }

        [DoNotSerialize]
        public ValueOutput ManipulationInput { get; private set; }

        protected override void Definition()
        {
            Manipulable = ValueInput<Manipulable>(nameof(Manipulable), null).NullMeansSelf();

            GameObjectReference = ValueOutput(nameof(GameObjectReference), (flow) => flow.GetValue<Manipulable>(Manipulable).InteractableRef.GameObjectRef);

            InteractionColliders = ValueOutput(nameof(InteractionColliders), (flow) => flow.GetValue<Manipulable>(Manipulable).InteractableRef.InteractionColliders);

            IsManipulated = ValueOutput(nameof(IsManipulated), (flow) => flow.GetValue<Manipulable>(Manipulable).CurrentInteractionState == EManipulableState.Manipulating);

            ManipulationInput = ValueOutput(nameof(ManipulationInput), (flow) => flow.GetValue<Manipulable>(Manipulable).CurrentManipulationInput);
        }
    }
}
