using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScriptingEditor
{
    [Descriptor(typeof(CheckPlatformUnit))]
    public class CheckPlatformDescriptor : UnitDescriptor<CheckPlatformUnit>
    {
        public CheckPlatformDescriptor(CheckPlatformUnit unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit allows to define a flow based on which platform the experience is running.";
        }
    }
}
