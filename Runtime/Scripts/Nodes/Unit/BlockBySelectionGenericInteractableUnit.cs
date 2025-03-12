using Reflectis.CreatorKit.Worlds.Core.Interaction;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Interactable: BlockInteractionBySelection")]
    [UnitSurtitle("Interactable")]
    [UnitShortTitle("BlockInteractionBySelection")]
    [UnitCategory("Reflectis\\Flow")]
    public class BlockBySelectionInteractableUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput Interactable { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput BlockValue { get; private set; }

        protected override void Definition()
        {
            Interactable = ValueInput<IInteractable>(nameof(Interactable));

            BlockValue = ValueInput<bool>(nameof(BlockValue), false);

            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {

                if (f.GetValue<bool>(BlockValue))
                {
                    f.GetValue<IInteractable>(Interactable).CurrentBlockedState |= IInteractable.EBlockedState.BlockedBySelection;
                }
                else
                {
                    f.GetValue<IInteractable>(Interactable).CurrentBlockedState = f.GetValue<IInteractable>(Interactable).CurrentBlockedState & ~IInteractable.EBlockedState.BlockedBySelection;
                }
                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);

        }
    }
}
