using Reflectis.SDK.Core.CharacterController;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;
using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Settings: Set default settings")]
    [UnitSurtitle("SetDefaultSettings")]
    [UnitShortTitle("Set Default Settings")]
    [UnitCategory("Reflectis\\Flow")]
    public class SetDefaultSettingsNode : Unit
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
                SM.GetSystem<ICharacterControllerSystem>().SetDefaultSettingsAsActive();
                return OutputTrigger;
            });
        
            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }
    }
}
