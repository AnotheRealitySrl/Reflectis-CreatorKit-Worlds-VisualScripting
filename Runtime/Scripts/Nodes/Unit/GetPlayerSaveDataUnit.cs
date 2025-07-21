using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Player Save Data: Get Data")]
    [UnitSurtitle("Player Save Data")]
    [UnitShortTitle("Get Player Save Data")]
    [UnitCategory("Reflectis\\Get")]
    public class GetPlayerSaveDataUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Key { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput Data { get; private set; }

        protected override void Definition()
        {
            Key = ValueInput(nameof(Key), string.Empty);

            Data = ValueOutput(nameof(Data),
                (f) =>
            {
                return SM.GetSystem<IClientModelSystem>().GetMySaveData(f.GetValue<string>(Key));
            });
        }
    }
}
