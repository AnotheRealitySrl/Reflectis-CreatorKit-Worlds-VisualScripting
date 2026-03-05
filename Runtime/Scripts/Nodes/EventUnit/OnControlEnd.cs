using Reflectis.CreatorKit.Worlds.Placeholders;
using Reflectis.SDK.Core.VisualScripting;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis ControlManager: On Control End")]
    [UnitSurtitle("Control Manager")]
    [UnitShortTitle("On Control End")]
    [UnitCategory("Events\\Reflectis")]
    public class OnControlEnd : UnityEventUnit<ControlManager>
    {
        protected override bool register => true;

        [DoNotSerialize]
        public ValueInput ControlManagerReference { get; private set; }

        protected override void Definition()
        {
            base.Definition();
            ControlManagerReference = ValueInput<ControlManager>(nameof(ControlManagerReference), null).NullMeansSelf();
        }

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook("InformativeItem_Abstract" + this.ToString().Split("EventUnit")[0]);
        }

        protected override UnityEvent GetEvent(GraphReference reference)
        {
            var controlManagerReference = Flow.New(reference).GetValue<ControlManager>(ControlManagerReference);
            if (controlManagerReference == null)
            {
                return new UnityEvent();
            }
            else
            {
                return controlManagerReference.onControlEnd;
            }
        }

        protected override ControlManager GetArguments(GraphReference reference)
        {
            return Flow.New(reference).GetValue<ControlManager>(ControlManagerReference);
        }
    }
}
