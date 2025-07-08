using Reflectis.SDK.Core.NetworkingSystem;
using Reflectis.SDK.Core.SystemFramework;

using System.Collections.Generic;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Networking: Get current network time")]
    [UnitSurtitle("NetworkTime")]
    [UnitShortTitle("Get Network Time")]
    [UnitCategory("Reflectis\\Get")]
    public class GetCurrentNetworkTime : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput NetworkedTime { get; private set; }

        private List<Flow> runningFlows = new List<Flow>();

        private double networkedTime;

        protected override void Definition()
        {
            NetworkedTime = ValueOutput<double>(nameof(NetworkedTime), f => SM.GetSystem<INetworkingSystem>().GetSharedNetworkTime());
        }

    }
}


