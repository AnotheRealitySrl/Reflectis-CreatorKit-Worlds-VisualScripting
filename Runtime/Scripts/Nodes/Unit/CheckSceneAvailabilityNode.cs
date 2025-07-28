using Reflectis.CreatorKit.Worlds.Core.ApplicationManagement;
using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;
using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Platform: Check Scene Availability")]
    [UnitSurtitle("Platform")]
    [UnitShortTitle("Check Scene Availability")]
    [UnitCategory("Reflectis\\Flow")]
    public class CheckSceneAvailabilityNode : AwaitableUnit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput SceneAddressableName { get; private set; }
        public ValueOutput IsAvailable { get; private set; }
        private bool _isAvailable;

        protected override void Definition()
        {
            SceneAddressableName = ValueInput<string>(nameof(SceneAddressableName));
            IsAvailable = ValueOutput<bool>(nameof(IsAvailable), f => _isAvailable);

            base.Definition();
        }

        protected override async Task AwaitableAction(Flow flow)
        {
            var clientModelSystem = SM.GetSystem<IClientModelSystem>();

            var experience = await clientModelSystem.GetExperienceByAddressableName(flow.GetValue<string>(SceneAddressableName));

            if (experience != null)
            {
                _isAvailable = true;
            }
            else
            {
                _isAvailable = false;
            }
        }
    }
}
