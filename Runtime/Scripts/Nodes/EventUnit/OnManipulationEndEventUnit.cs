using Unity.VisualScripting;

using static Reflectis.CreatorKit.Worlds.Core.Interaction.IManipulable;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Manipulable: On Manipulation End")]
    [UnitSurtitle("Manipulable")]
    [UnitShortTitle("On Manipulation End")]
    [UnitCategory("Events\\Reflectis")]
    public class OnManipulationEndEventUnit : OnManipulationEventUnit
    {
        protected override void OnManipulableStateChange(EManipulableState manipulableState)
        {
            if (manipulableState == EManipulableState.Idle)
            {
                Trigger(graphReference, manipulableReference);
            }
        }

    }
}
