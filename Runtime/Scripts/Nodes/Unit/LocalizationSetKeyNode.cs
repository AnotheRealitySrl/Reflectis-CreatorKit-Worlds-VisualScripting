﻿using Reflectis.CreatorKit.Worlds.Placeholders;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Localization: Set Key")]
    [UnitSurtitle("Localization")]
    [UnitShortTitle("Set Key")]
    [UnitCategory("Reflectis\\Flow")]
    public class LocalizationSetKeyNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput LocalizationPlaceholder { get; private set; }
        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput NewKey { get; private set; }

        protected override void Definition()
        {
            LocalizationPlaceholder = ValueInput<LocalizationPlaceholder>(nameof(LocalizationPlaceholder), null).NullMeansSelf();
            NewKey = ValueInput<string>(nameof(NewKey), string.Empty);

            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {
                f.GetValue<LocalizationPlaceholder>(LocalizationPlaceholder).SetKey(f.GetValue<string>(NewKey));
                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }
    }
}
