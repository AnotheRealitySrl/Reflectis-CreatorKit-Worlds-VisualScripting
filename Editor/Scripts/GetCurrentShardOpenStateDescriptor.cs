using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScriptingEditor
{
    [Descriptor(typeof(GetCurrentShardOpenStateNode))]
    public class GetCurrentShardOpenStateDescriptor : UnitDescriptor<GetCurrentShardOpenStateNode>
    {
        public GetCurrentShardOpenStateDescriptor(GetCurrentShardOpenStateNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit will retrieve the open state of the Reflectis event shard " +
                "where the local player currently is.";
        }

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);

            switch (port.key)
            {
                case "IsOpen":
                    description.summary = "Returns true if the shard is open. Returns false if the shard is closed";
                    break;
            }
        }
    }
}
