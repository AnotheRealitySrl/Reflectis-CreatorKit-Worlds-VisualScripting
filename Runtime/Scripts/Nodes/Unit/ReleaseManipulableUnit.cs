using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.CreatorKit.Worlds.Placeholders;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Manipulable: Release Manipulable")]
    [UnitSurtitle("Manipulable")]
    [UnitShortTitle("Release Manipulable")]
    [UnitCategory("Reflectis\\Flow")]
    public class ReleaseManipulableUnit : Unit
    {
        [DoNotSerialize]
        public ValueInput Manipulable { get; private set; }

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
            Manipulable = ValueInput<ManipulablePlaceholder>(nameof(Manipulable), null).NullMeansSelf();
            Succession(inputTrigger, outputTrigger);

        }

        private ControlOutput Output(Flow flow)
        {
            flow.GetValue<ManipulablePlaceholder>(Manipulable).gameObject.GetComponent<IManipulable>().ForceGrabRelease();
            return outputTrigger;
        }
    }

}
