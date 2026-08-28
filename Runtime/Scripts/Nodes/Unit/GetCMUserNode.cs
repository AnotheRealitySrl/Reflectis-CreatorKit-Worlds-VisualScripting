using Virtuademy.CreatorKit.Worlds.Core.ClientModels;
using Virtuademy.SDK.Core.SystemFramework;

using Unity.VisualScripting;

namespace Virtuademy.CreatorKit.Worlds.VisualScripting
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
