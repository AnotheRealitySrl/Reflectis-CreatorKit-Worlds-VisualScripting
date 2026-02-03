using Reflectis.CreatorKit.Worlds.Core.ApplicationManagement;
using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;

using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Platform: Reload Scene")]
    [UnitSurtitle("Platform")]
    [UnitShortTitle("Reload Scene")]
    [UnitCategory("Reflectis\\Flow")]
    public class ReloadSceneNode : AwaitableUnit
    {
        public CMEnvironment SceneAddressableName { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput IsTenantEnvironment { get; private set; }

        protected override void Definition()
        {
            SceneAddressableName = SM.GetSystem<IClientModelSystem>().CurrentSession.Experience.Environment;
            IsTenantEnvironment = ValueInput<bool>(nameof(IsTenantEnvironment), false);

            base.Definition();
        }

        protected override async Task AwaitableAction(Flow flow)
        {
            var clientModelSystem = SM.GetSystem<IClientModelSystem>();

            var experience = await clientModelSystem.GetExperienceByAddressableName(SM.GetSystem<IClientModelSystem>().CurrentSession.Experience.Environment.Name, flow.GetValue<bool>(IsTenantEnvironment));

            if (experience != null)
            {
                await IReflectisApplicationManager.Instance.JoinExperience(experience, true);
            }
            else
            {
                Debug.LogError($"[Reflectis Creator Kit | Change Scene node] The key specified {SceneAddressableName.Name} " +
                    $"for the environment is not correct or the experience is not flagged as " +
                    $"public");
            }
        }
    }
}
