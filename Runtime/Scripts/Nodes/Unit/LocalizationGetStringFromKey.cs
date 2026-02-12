using Reflectis.CreatorKit.Worlds.Core.Localization;
using Reflectis.CreatorKit.Worlds.Placeholders;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Localization: Get translation")]
    [UnitSurtitle("Localization")]
    [UnitShortTitle("Get Translation")]
    [UnitCategory("Reflectis\\Flow")]
    public class LocalizationGetStringFromKey : Unit
    {
        [NullMeansSelf]

        [DoNotSerialize]
        public ValueInput Key { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueOutput Translation { get; private set; }


        protected override void Definition()
        {
            Key = ValueInput<string>(nameof(Key), string.Empty);

            Translation = ValueOutput<string>(nameof(Translation), (f) =>
                {
                    return SM.GetSystem<ILocalizationSystem>().GetStringFromExternalKey(f.GetValue<string>(Key));
                });
        }
    }
}
