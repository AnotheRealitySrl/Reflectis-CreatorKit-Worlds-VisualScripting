using Reflectis.CreatorKit.Worlds.Core.Interaction;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis VisualScriptingInteractable: BlockInteractionBySelection")]
    [UnitSurtitle("VisualScriptingInteractable")]
    [UnitShortTitle("BlockInteractionBySelection")]
    [UnitCategory("Reflectis\\Flow")]
    public class BlockBySelectionVisualScriptingInteractableUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput VisualScriptingInteractable { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput BlockValue { get; private set; }

        protected override void Definition()
        {
            VisualScriptingInteractable = ValueInput<IVisualScriptingInteractable>(nameof(VisualScriptingInteractable));

            BlockValue = ValueInput<bool>(nameof(BlockValue), false);

            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {

                if (f.GetValue<bool>(BlockValue))
                {
                    f.GetValue<IVisualScriptingInteractable>(VisualScriptingInteractable).CurrentBlockedState |= InteractableBehaviourBase.EBlockedState.BlockedBySelection;
                }
                else
                {
                    f.GetValue<IVisualScriptingInteractable>(VisualScriptingInteractable).CurrentBlockedState = f.GetValue<IVisualScriptingInteractable>(VisualScriptingInteractable).CurrentBlockedState & ~InteractableBehaviourBase.EBlockedState.BlockedBySelection;
                }
                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);

        }
    }
}
