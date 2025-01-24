using Reflectis.CreatorKit.Worlds.VisualScripting.ClientModels;
using Reflectis.SDK.Core;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis CMEnvironment: Get CMEnvironment")]
    [UnitSurtitle("CMEnvironment")]
    [UnitShortTitle("Get CMEnvironment")]
    [UnitCategory("Reflectis\\Get")]
    public class GetCMEnvironmentNode : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput CMEnvironment { get; private set; }

        protected override void Definition()
        {
            CMEnvironment = ValueOutput(nameof(CMEnvironment), (f) => SM.GetSystem<IClientModelSystem>().CurrentEvent.Environment);
        }
    }
}
