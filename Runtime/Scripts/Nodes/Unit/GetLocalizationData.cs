using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.CreatorKit.Worlds.Core.Localization;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Localization: Get Localization Data")]
    [UnitSurtitle("LocalizationData")]
    [UnitShortTitle("Get LocalizationData")]
    [UnitCategory("Reflectis\\Get")]
    public class GetLocalizationData : Unit
    {
        public ValueOutput CurrentLanguage { get; private set; }
        public ValueOutput CurrentLanguageCode { get; private set; }
        public ValueOutput LanguageList { get; private set; }

        protected override void Definition()
        {
            CurrentLanguage = ValueOutput<string>(nameof(CurrentLanguage), (flow) => SM.GetSystem<ILocalizationSystem>().GetCurrentLocalization());
            CurrentLanguageCode = ValueOutput<string>(nameof(CurrentLanguageCode), (flow) => SM.GetSystem<ILocalizationSystem>().GetCurrentLanguageCode());
            LanguageList = ValueOutput<List<string>>(nameof(LanguageList), f => SM.GetSystem<ILocalizationSystem>().GetLanguagesList());
        }

    }
}
