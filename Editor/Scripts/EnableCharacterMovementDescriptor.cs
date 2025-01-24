using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScriptingEditor
{
    [Descriptor(typeof(EnableCharacterMovementNode))]
    public class EnableCharacterMovementDescriptor : UnitDescriptor<EnableCharacterMovementNode>
    {
        public EnableCharacterMovementDescriptor(EnableCharacterMovementNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit will disable or enable the character movement. Remember to reenable the character movement" +
                " after the interaction is completed.";
        }
    }
}
