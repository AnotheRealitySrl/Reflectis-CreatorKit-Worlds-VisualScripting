using Virtuademy.SDK.Core.SystemFramework;
using Virtuademy.SDK.Core.CharacterController;
using Virtuademy.SDK.Core.VisualScripting;

using System.Threading.Tasks;

using Unity.VisualScripting;

namespace Virtuademy.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Character: Exit Pan")]
    [UnitSurtitle("Character")]
    [UnitShortTitle("Exit Pan")]
    [UnitCategory("Reflectis\\Flow")]
    public class ExitPanNode : AwaitableUnit
    {
        protected async override Task AwaitableAction(Flow flow)
        {
            await SM.GetSystem<ICharacterControllerSystem>().GoToSetMovementState();
        }
    }
}
