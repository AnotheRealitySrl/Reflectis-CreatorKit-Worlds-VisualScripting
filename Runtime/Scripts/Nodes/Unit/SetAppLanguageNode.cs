using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;
using Reflectis.CreatorKit.Worlds.Core.Localization;

using System.Collections.Generic;
using System.Threading.Tasks;

using Unity.VisualScripting;
using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Localization: Set Language")]
    [UnitSurtitle("SetLanguage")]
    [UnitShortTitle("Set Language")]
    [UnitCategory("Reflectis\\Flow")]
    public class SetAppLanguageNode : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput LanguageChoice { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        protected override void Definition()
        {
            LanguageChoice = ValueInput<string>(nameof(LanguageChoice));

            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {
                Debug.LogError("Language selected is " + LanguageChoice);
                SM.GetSystem<ILocalizationSystem>().SetLanguage(f.GetValue<string>(LanguageChoice));

                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }

    }
}
