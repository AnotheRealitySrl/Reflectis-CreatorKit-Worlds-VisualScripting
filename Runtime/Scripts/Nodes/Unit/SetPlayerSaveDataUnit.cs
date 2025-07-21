using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Player Save Data: Set Data")]
    [UnitSurtitle("Player Save Data")]
    [UnitShortTitle("Set Player Save Data")]
    [UnitCategory("Reflectis\\Flow")]
    public class SetPlayerSaveDataUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput Key { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput Value { get; private set; }

        protected override void Definition()
        {
            Key = ValueInput<string>(nameof(Key), string.Empty);

            Value = ValueInput<object>(nameof(Value), null);

            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {
                SM.GetSystem<IClientModelSystem>().SetMySaveData(
                    f.GetValue<string>(Key),
                    f.GetValue<object>(Value));
                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }
    }
}
