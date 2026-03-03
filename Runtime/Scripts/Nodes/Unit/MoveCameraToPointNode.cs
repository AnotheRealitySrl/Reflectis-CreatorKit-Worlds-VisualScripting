using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.CharacterController;
using Reflectis.SDK.Core.VisualScripting;

using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Character: Move camera to point")]
    [UnitSurtitle("Character")]
    [UnitShortTitle("Move camera")]
    [UnitCategory("Reflectis\\Flow")]
    public class MoveCameraToPointNode : AwaitableUnit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput TargetTransform { get; private set; }

        protected override void Definition()
        {
            TargetTransform = ValueInput<Transform>(nameof(TargetTransform));

            base.Definition();
        }

        protected override async Task AwaitableAction(Flow flow)
        {
            await SM.GetSystem<ICharacterControllerSystem>().MoveCameraToPoint(flow.GetValue<Transform>(TargetTransform));
        }
    }
}
