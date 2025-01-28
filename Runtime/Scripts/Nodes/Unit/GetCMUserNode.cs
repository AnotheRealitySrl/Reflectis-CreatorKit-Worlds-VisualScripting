using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis CMUser: Get CMUser")]
    [UnitSurtitle("CMUser")]
    [UnitShortTitle("Get CMUser")]
    [UnitCategory("Reflectis\\Get")]
    public class GetCMUserNode : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput CMUser { get; private set; }

        protected override void Definition()
        {
            CMUser = ValueOutput(nameof(CMUser), (f) => SM.GetSystem<IClientModelSystem>().UserData);
        }
    }
}
