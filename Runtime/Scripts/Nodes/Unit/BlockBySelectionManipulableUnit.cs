using Reflectis.CreatorKit.Worlds.Core.Interaction;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Manipulable: BlockInteractionBySelection")]
    [UnitSurtitle("Manipulable")]
    [UnitShortTitle("BlockInteractionBySelection")]
    [UnitCategory("Reflectis\\Flow")]
    public class BlockBySelectionManipulableUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput Manipulable { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput BlockValue { get; private set; }

        protected override void Definition()
        {
            Manipulable = ValueInput<IManipulable>(nameof(Manipulable));

            BlockValue = ValueInput<bool>(nameof(BlockValue), false);

            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {

                if (f.GetValue<bool>(BlockValue))
                {
                    f.GetValue<IManipulable>(Manipulable).CurrentBlockedState |= InteractableBehaviourBase.EBlockedState.BlockedBySelection;
                }
                else
                {
                    f.GetValue<IManipulable>(Manipulable).CurrentBlockedState = f.GetValue<IManipulable>(Manipulable).CurrentBlockedState & ~InteractableBehaviourBase.EBlockedState.BlockedBySelection;
                }
                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);

        }
    }
}
