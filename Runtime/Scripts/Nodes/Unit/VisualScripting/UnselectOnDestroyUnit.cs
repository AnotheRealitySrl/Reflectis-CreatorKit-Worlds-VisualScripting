using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.SDK.Core.VisualScripting;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Generic Interactable: Unselect OnDestroy")]
    [UnitSurtitle("Generic Interactable")]
    [UnitShortTitle("Unselect OnDestroy")]
    [UnitCategory("Events\\Reflectis")]
    public class UnselectOnDestroyUnit : AwaitableEventUnit<IVisualScriptingInteractable>
    {
        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook("VisualScriptingInteractable" + this.ToString().Split("Unit")[0]);
        }
    }
}
