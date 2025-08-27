using Reflectis.CreatorKit.Worlds.Core.Help;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Tutorial: On Tutorial Closed")]
    [UnitSurtitle("Tutorial")]
    [UnitShortTitle("On Tutorial Closed")]
    [UnitCategory("Events\\Reflectis")]
    public class OnTutorialCloseEventUnit : UnityEventUnit<Null>
    {

        public static string eventName = "OnTutorialClosed";

        protected override bool register => true;

        protected override void Definition()
        {
            base.Definition();
        }

        public override EventHook GetHook(GraphReference reference)
        {

            return new EventHook("tutorial closed" + this.ToString().Split("EventUnit")[0]);
        }

        protected override UnityEvent GetEvent(GraphReference reference)
        {
            if (SM.GetSystem<IHelpSystem>() != null)
            {
            }
            else
            {
                return null;
            }
            return SM.GetSystem<IHelpSystem>().OnFinishedClosing;
        }

        public override void Uninstantiate(GraphReference instance)
        {
            base.Uninstantiate(instance);
        }

        protected override Null GetArguments(GraphReference reference)
        {
            return null;
        }
    }
}
