using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Splines;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Splines
{
    [UnitTitle("Reflectis Spline: Get SplineContainer")]
    [UnitSurtitle("Spline")]
    [UnitShortTitle("Get SplineContainer")]
    [UnitCategory("Reflectis\\Get")]

    public class GetSplineContainerUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput ObjInput { get; private set; }

        [DoNotSerialize]
        public ValueOutput SplineContainer { get; private set; }

        protected override void Definition()
        {
            ObjInput = ValueInput<GameObject>(nameof(ObjInput), null).NullMeansSelf();

            SplineContainer = ValueOutput(nameof(SplineContainer), (flow) => flow.GetValue<GameObject>(ObjInput).GetComponent<SplineContainer>());
        }
    }
}