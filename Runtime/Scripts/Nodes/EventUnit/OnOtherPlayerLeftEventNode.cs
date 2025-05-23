using Reflectis.SDK.Core;
using Reflectis.SDK.Core.NetworkingSystem;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Networking: On Other Player Left")]
    [UnitSurtitle("Networking")]
    [UnitShortTitle("On Other Player Left")]
    [UnitCategory("Events\\Reflectis")]
    public class OnOtherPlayerLeftEventNode : UnityEventUnit<(int, string), PlayerData>
    {
        public static string eventName = "NetworkingOnOtherPlayerLeft";

        [DoNotSerialize]
        public ValueOutput UserId { get; private set; }
        [DoNotSerialize]
        public ValueOutput SessionId { get; private set; }
        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(eventName);
        }

        protected override void Definition()
        {
            base.Definition();
            UserId = ValueOutput<int>(nameof(UserId));
            SessionId = ValueOutput<string>(nameof(SessionId));
        }

        protected override void AssignArguments(Flow flow, (int, string) args)
        {
            flow.SetValue(UserId, args.Item1);
            flow.SetValue(SessionId, args.Item2);
        }

        protected override UnityEvent<PlayerData> GetEvent(GraphReference reference)
        {
            return SM.GetSystem<INetworkingSystem>().OnOtherPlayerLeaveShard;
        }

        protected override (int, string) GetArguments(GraphReference reference, PlayerData eventData)
        {
            return (eventData.UserId, eventData.SessionId);
        }
    }
}
