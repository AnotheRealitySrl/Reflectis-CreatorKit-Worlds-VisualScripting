using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.NetworkingSystem;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Networking: Get Current Shard Open State")]
    [UnitSurtitle("Networking")]
    [UnitShortTitle("Get Current Shard Open State")]
    [UnitCategory("Reflectis\\Get")]
    public class GetCurrentShardOpenStateNode : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabel("Is Open")]
        public ValueOutput IsOpen { get; private set; }

        protected override void Definition()
        {
            IsOpen = ValueOutput(nameof(IsOpen), (f) => SM.GetSystem<INetworkingSystem>().IsCurrentShardOpen);
        }
    }
}
