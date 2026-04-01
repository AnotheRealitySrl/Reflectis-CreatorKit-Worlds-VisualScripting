using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.CharacterController;
using Reflectis.SDK.Core.VisualScripting;

using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Character: FreeCameraPan")]
    [UnitSurtitle("Character")]
    [UnitShortTitle("FreeCameraPan")]
    [UnitCategory("Reflectis\\Flow")]
    public class FreeCameraPanNode : AwaitableUnit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput TargetTransform { get; private set; }

        [NullMeansSelf]
        public ValueInput MaxZoom { get; private set; }

        [NullMeansSelf]
        public ValueInput MinZoom { get; private set; }

        [NullMeansSelf]
        public ValueInput MaxYRotation { get; private set; }

        [NullMeansSelf]
        public ValueInput MinYRotation { get; private set; }

        protected override void Definition()
        {
            TargetTransform = ValueInput<Transform>(nameof(TargetTransform));
            MaxZoom = ValueInput<float>(nameof(MaxZoom), 0.0001f);
            MinZoom = ValueInput<float>(nameof(MinZoom), 1f);
            MaxYRotation = ValueInput<float>(nameof(MaxYRotation), 45f);
            MinYRotation = ValueInput<float>(nameof(MinYRotation), -45f);

            base.Definition();
        }

        protected override async Task AwaitableAction(Flow flow)
        {
            await SM.GetSystem<ICharacterControllerSystem>().GoToInteractState(flow.GetValue<Transform>(TargetTransform), flow.GetValue<float>(MaxZoom),
                flow.GetValue<float>(MinZoom), flow.GetValue<float>(MaxYRotation), flow.GetValue<float>(MinYRotation), true);
        }
    }
}
