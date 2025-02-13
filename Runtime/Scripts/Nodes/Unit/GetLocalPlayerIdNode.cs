using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Networking: Get Local Player ID")]
    [UnitSurtitle("Networking")]
    [UnitShortTitle("Get Local Player ID")]
    [UnitCategory("Reflectis\\Get")]
    public class GetLocalPlayerIdNode : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabel("PlayerId")]
        public ValueOutput SessionId { get; private set; }

        protected override void Definition()
        {
            SessionId = ValueOutput(nameof(SessionId), (f) => SM.GetSystem<IClientModelSystem>().SessionId);
        }
    }
}
