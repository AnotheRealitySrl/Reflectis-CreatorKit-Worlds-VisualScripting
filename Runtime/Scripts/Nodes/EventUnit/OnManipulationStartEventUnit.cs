using Unity.VisualScripting;

using static Reflectis.CreatorKit.Worlds.Core.Interaction.IManipulable;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Manipulable: On Manipulation Start")]
    [UnitSurtitle("Manipulable")]
    [UnitShortTitle("On Manipulation Start")]
    [UnitCategory("Events\\Reflectis")]
    public class OnManipulationStartEventUnit : OnManipulationEventUnit
    {
        protected override bool ShouldTriggerOnChange(EManipulableState manipulableState)
        {
            return manipulableState == EManipulableState.Manipulating;
        }
    }
}
