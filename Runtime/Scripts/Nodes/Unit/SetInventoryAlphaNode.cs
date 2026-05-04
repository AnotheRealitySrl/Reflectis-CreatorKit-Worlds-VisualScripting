using Reflectis.CreatorKit.Worlds.Placeholders;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Inventory: Set Alpha")]
    [UnitSurtitle("SetAlpha")]
    [UnitShortTitle("Set Alpha")]
    [UnitCategory("Reflectis\\Flow")]
    public class SetInventoryAlphaNode : Unit
    {
        [DoNotSerialize]
        public ValueInput Alpha { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        protected override void Definition()
        {
            Alpha = ValueInput<float>(nameof(Alpha));

            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {
                SM.GetSystem<IInventorySystem>().SetInventoryAlpha(f.GetValue<float>(Alpha));

                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }

    }
}
