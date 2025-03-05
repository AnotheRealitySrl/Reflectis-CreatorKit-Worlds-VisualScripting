using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Splines;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Splines
{
    [UnitTitle("Reflectis Spline: Get SplineAnimate")]
    [UnitSurtitle("Spline")]
    [UnitShortTitle("Get SplineAnimate")]
    [UnitCategory("Reflectis\\Get")]

    public class GetSplineAnimateUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput ObjInput { get; private set; }

        [DoNotSerialize]
        public ValueOutput SplineAnimate { get; private set; }

        protected override void Definition()
        {
            ObjInput = ValueInput<GameObject>(nameof(ObjInput), null).NullMeansSelf();

            SplineAnimate = ValueOutput(nameof(SplineAnimate), (flow) => flow.GetValue<GameObject>(ObjInput).GetComponent<SplineAnimate>());
        }
    }
}