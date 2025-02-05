using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.SDK.Core.SystemFramework;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis ContextualMenu: Hide")]
    [UnitSurtitle("ContextualMenu")]
    [UnitShortTitle("Hide")]
    [UnitCategory("Reflectis\\Flow")]

    public class HideContextualMenuUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        protected override void Definition()
        {
            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {
                SM.GetSystem<IContextualMenuSystem>().HideContextualMenu();
                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }


    }
}
