using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.NetworkingSystem;
using Reflectis.SDK.Core.SystemFramework;

using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

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

        protected override void Definition()
        {
            if (SM.GetSystem<IClientModelSystem>().CurrentSession.Multiplayer)
                NetworkedTime = ValueOutput<double>(nameof(NetworkedTime), f => SM.GetSystem<INetworkingSystem>().GetSharedNetworkTime());
            else
                NetworkedTime = ValueOutput<double>(nameof(NetworkedTime), f => Time.time);
        }

    }
}


