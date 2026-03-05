using Reflectis.CreatorKit.Worlds.Placeholders;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis ControlManager: Expose Control Manager")]
    [UnitSurtitle("Expose")]
    [UnitShortTitle("Control Manager Instance")]
    [UnitCategory("Reflectis\\Expose")]
    public class ExposeControlManagerUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput ControlManager { get; private set; }
      
        [DoNotSerialize]
        public ValueOutput InformativeItemList { get; private set; }

        protected override void Definition()
        {
            ControlManager = ValueInput<ControlManager>(nameof(ControlManager), null).NullMeansSelf();

            InformativeItemList = ValueOutput(nameof(InformativeItemList), (flow) => flow.GetValue<ControlManager>(ControlManager).informativeItems);
        }
    }
}
