using Reflectis.SDK.Core;
using Reflectis.SDK.Core.NetworkingSystem;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Networking: On Other Player Left")]
    [UnitSurtitle("Networking")]
    [UnitShortTitle("On Other Player Left")]
    [UnitCategory("Events\\Reflectis")]
    public class OnOtherPlayerLeftEventNode : EventUnit<(int, string)>
    {
        public static string eventName = "NetworkingOnOtherPlayerLeft";

        [DoNotSerialize]
        public ValueOutput UserId { get; private set; }
        [DoNotSerialize]
        public ValueOutput SessionId { get; private set; }
        protected override bool register => true;

        protected GraphReference graphReference;

        public override EventHook GetHook(GraphReference reference)
        {
            graphReference = reference;

            return new EventHook(eventName);
        }

        public override void Instantiate(GraphReference instance)
        {
            base.Instantiate(instance);

            SM.GetSystem<INetworkingSystem>().OnOtherPlayerLeaveShard.AddListener(OnPlayerLeft);
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

        private void OnPlayerLeft(PlayerData playerData)
        {
            Trigger(graphReference, (playerData.UserId, playerData.SessionId));
        }

        public override void Uninstantiate(GraphReference instance)
        {
            base.Uninstantiate(instance);
            SM.GetSystem<INetworkingSystem>().OnOtherPlayerLeaveShard.RemoveListener(OnPlayerLeft);
        }
    }
}
