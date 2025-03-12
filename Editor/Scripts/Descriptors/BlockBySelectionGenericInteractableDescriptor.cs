using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Editor
{
    [Descriptor(typeof(BlockBySelectionInteractableUnit))]
    public class BlockBySelectionInteractableDescriptor : UnitDescriptor<BlockBySelectionInteractableUnit>
    {
        public BlockBySelectionInteractableDescriptor(BlockBySelectionInteractableUnit unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit will enable or disable the interaction with a given Interactable element.";
        }
    }
}
