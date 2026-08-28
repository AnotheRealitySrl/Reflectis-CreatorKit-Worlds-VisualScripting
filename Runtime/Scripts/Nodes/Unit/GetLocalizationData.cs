using Virtuademy.CreatorKit.Worlds.Core.ClientModels;
using Virtuademy.CreatorKit.Worlds.Core.Localization;
using Virtuademy.SDK.Core.SystemFramework;
using Virtuademy.SDK.Core.VisualScripting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Unity.VisualScripting;

namespace Virtuademy.CreatorKit.Worlds.VisualScripting
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
