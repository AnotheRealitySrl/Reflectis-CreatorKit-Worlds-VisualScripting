using Virtuademy.CreatorKit.Worlds.Placeholders;
using Virtuademy.SDK.Core.SystemFramework;

using Unity.VisualScripting;
using UnityEngine;



namespace Virtuademy.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis general: Spawn Feedback (WebGL only)")]
    [UnitSurtitle("General")]
    [UnitShortTitle("SpawnFeedback (WebGL only)")]
    [UnitCategory("Reflectis\\Flow")]
    public class SpawnFeedbackCheckNode : Unit
    {
        [NullMeansSelf]
        public ValueInput Correctness { get; private set; }
        [NullMeansSelf]
        public ValueInput SpawnTransform { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput inputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput outputTrigger { get; private set; }

        protected override void Definition()
        {
            inputTrigger = ControlInput(nameof(inputTrigger), Output);
            outputTrigger = ControlOutput("outputTrigger");

            Correctness = ValueInput<bool>(nameof(Correctness), true);
            SpawnTransform = ValueInput<Transform>(nameof(SpawnTransform), null);

            Succession(inputTrigger, outputTrigger);
            Succession(inputTrigger, outputTrigger);

        }

        private ControlOutput Output(Flow flow)
        {
            SM.GetSystem<IEquippableSystem>().DisplayFeedback(flow.GetValue<Transform>(SpawnTransform), flow.GetValue<bool>(Correctness));
            return outputTrigger;
        }
    }
}