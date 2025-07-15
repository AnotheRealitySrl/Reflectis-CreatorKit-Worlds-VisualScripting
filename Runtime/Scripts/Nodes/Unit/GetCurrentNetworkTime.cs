using Reflectis.CreatorKit.Worlds.Core.ApplicationManagement;
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
            NetworkedTime = ValueOutput<double>(nameof(NetworkedTime), f =>
            {
                if (IReflectisApplicationManager.Instance.State == SDK.Core.ApplicationManagement.EApplicationState.Online
                && SM.GetSystem<IClientModelSystem>().CurrentSession.Multiplayer)
                {
                    return SM.GetSystem<INetworkingSystem>().GetSharedNetworkTime();
                }
                return Time.time;
            });

        }
    }
}


