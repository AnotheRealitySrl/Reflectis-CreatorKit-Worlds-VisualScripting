using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Editor
{
    [Descriptor(typeof(SetThirdPersonCameraModeNode))]
    public class SetThirdPersonCameraModeDescriptor : UnitDescriptor<SetThirdPersonCameraModeNode>
    {
        public SetThirdPersonCameraModeDescriptor(SetThirdPersonCameraModeNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit will switch the character view to third person (which is the " +
                "default mode). This is effective only on WebGL/Desktop platform.";
        }
    }
}
