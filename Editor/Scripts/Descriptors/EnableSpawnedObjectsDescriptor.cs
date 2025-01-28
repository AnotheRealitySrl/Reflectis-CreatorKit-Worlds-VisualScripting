using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Editor
{
    [Descriptor(typeof(EnableSpawnedObjectsNode))]
    public class EnableSpawnedObjectsDescriptor : UnitDescriptor<EnableSpawnedObjectsNode>
    {
        public EnableSpawnedObjectsDescriptor(EnableSpawnedObjectsNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "NOT IMPLEMENTED YET!";
        }
    }
}
