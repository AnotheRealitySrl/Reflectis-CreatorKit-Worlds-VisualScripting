using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScriptingEditor
{
    [Descriptor(typeof(GetLocalPlayerIdNode))]
    public class GetLocalPlayerIdDescriptor : UnitDescriptor<GetLocalPlayerIdNode>
    {
        public GetLocalPlayerIdDescriptor(GetLocalPlayerIdNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit will retrieve local's player current player id value.";
        }

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);

            switch (port.key)
            {
                case "PlayerId":
                    description.summary = "Unique identifier assigned to the player for the current shard.";
                    break;
            }
        }
    }
}
