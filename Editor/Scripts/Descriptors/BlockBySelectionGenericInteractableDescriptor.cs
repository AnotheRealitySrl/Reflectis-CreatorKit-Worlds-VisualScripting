using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Editor
{
    [Descriptor(typeof(BlockBySelectionVisualScriptingInteractableUnit))]
    public class BlockBySelectionVisualScriptingInteractableDescriptor : UnitDescriptor<BlockBySelectionVisualScriptingInteractableUnit>
    {
        public BlockBySelectionVisualScriptingInteractableDescriptor(BlockBySelectionVisualScriptingInteractableUnit unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit will enable or disable the interaction with a given VisualScriptingInteractable element.";
        }
    }
}
