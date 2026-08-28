using Virtuademy.CreatorKit.Worlds.Core.ClientModels;
using Virtuademy.SDK.Core.SystemFramework;

using Unity.VisualScripting;

namespace Virtuademy.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis CMEvent: Get CMEvent")]
    [UnitSurtitle("CMEvent")]
    [UnitShortTitle("Get CMEvent")]
    [UnitCategory("Reflectis\\Get")]
    public class GetCMEventNode : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput CMEvent { get; private set; }

        protected override void Definition()
        {
            CMEvent = ValueOutput(nameof(CMEvent), (f) => SM.GetSystem<IClientModelSystem>().CurrentSession);
        }
    }
}
