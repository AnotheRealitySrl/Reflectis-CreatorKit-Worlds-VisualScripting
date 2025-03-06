﻿using Unity.VisualScripting;

using UnityEngine.Splines;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Splines
{
    [UnitTitle("Reflectis: Expose SplineAnimate")]
    [UnitSurtitle("Expose")]
    [UnitShortTitle("SplineAnimate")]
    [UnitCategory("Reflectis\\Expose")]
    public class ExposeSplineAnimateUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput ObjInput { get; private set; }

        [DoNotSerialize] public ValueOutput Container { get; private set; }
        [DoNotSerialize] public ValueOutput NormalizedTime { get; private set; }
        [DoNotSerialize] public ValueOutput ElapsedTime { get; private set; }
        [DoNotSerialize] public ValueOutput Duration { get; private set; }
        [DoNotSerialize] public ValueOutput IsPlaying { get; private set; }

        protected override void Definition()
        {
            ObjInput = ValueInput<SplineAnimate>(nameof(ObjInput), null).NullMeansSelf();

            Container = ValueOutput(nameof(Container), (flow) => flow.GetValue<SplineAnimate>(ObjInput).Container);
            NormalizedTime = ValueOutput(nameof(NormalizedTime), (flow) => flow.GetValue<SplineAnimate>(ObjInput).NormalizedTime);
            ElapsedTime = ValueOutput(nameof(ElapsedTime), (flow) => flow.GetValue<SplineAnimate>(ObjInput).ElapsedTime);
            IsPlaying = ValueOutput(nameof(Duration), (flow) => flow.GetValue<SplineAnimate>(ObjInput).IsPlaying);
        }
    }
}

