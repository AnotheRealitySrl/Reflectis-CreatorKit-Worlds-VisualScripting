using Reflectis.CreatorKit.Worlds.Core.ApplicationManagement;
using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;

using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Platform: Change Scene")]
    [UnitSurtitle("Platform")]
    [UnitShortTitle("Change Scene")]
    [UnitCategory("Reflectis\\Flow")]
    public class ChangeSceneNode : AwaitableUnit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput SceneAddressableName { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput IsTenantEnvironment { get; private set; }

        protected override void Definition()
        {
            SceneAddressableName = ValueInput<string>(nameof(SceneAddressableName));
            IsTenantEnvironment = ValueInput<bool>(nameof(IsTenantEnvironment), false);

            base.Definition();
        }

        protected override async Task AwaitableAction(Flow flow)
        {
            var clientModelSystem = SM.GetSystem<IClientModelSystem>();

            var experience = await clientModelSystem.GetExperienceByAddressableName(flow.GetValue<string>(SceneAddressableName), flow.GetValue<bool>(IsTenantEnvironment));

            if (experience != null)
            {
                await IReflectisApplicationManager.Instance.JoinExperience(experience, true);
            }
            else
            {
                Debug.LogError($"[Reflectis Creator Kit | Change Scene node] The key specified {flow.GetValue<string>(SceneAddressableName)} " +
                    $"for the environment is not correct or the experience is not flagged as " +
                    $"public");
            }
        }
    }
}
