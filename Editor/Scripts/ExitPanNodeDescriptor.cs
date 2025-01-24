using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScriptingEditor
{
    [Descriptor(typeof(ExitPanNode))]
    public class ExitPanNodeDescriptor : UnitDescriptor<ExitPanNode>
    {
        public ExitPanNodeDescriptor(ExitPanNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit requires a coroutine flow to run properly. " +
                "This unit will reset the camera onto the character if it was previously locked by a pan.";
        }

    }
}
