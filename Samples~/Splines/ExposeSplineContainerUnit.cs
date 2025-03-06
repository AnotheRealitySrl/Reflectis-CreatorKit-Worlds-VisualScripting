using Unity.VisualScripting;

using UnityEngine.Splines;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Splines
{
    [UnitTitle("Reflectis: Expose SplineContainer")]
    [UnitSurtitle("Expose")]
    [UnitShortTitle("SplineContainer")]
    [UnitCategory("Reflectis\\Expose")]
    public class ExposeSplineContainerUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput ObjInput { get; private set; }

        [DoNotSerialize] public ValueOutput Splines { get; private set; }

        protected override void Definition()
        {
            ObjInput = ValueInput<SplineContainer>(nameof(ObjInput), null).NullMeansSelf();

            Splines = ValueOutput(nameof(Splines), (flow) => flow.GetValue<SplineContainer>(ObjInput).Splines);
        }
    }
}