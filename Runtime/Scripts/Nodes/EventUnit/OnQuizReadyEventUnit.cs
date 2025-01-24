﻿using Reflectis.SDK.Core.VisualScripting;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Quiz: On Quiz Ready")]
    [UnitSurtitle("Quiz")]
    [UnitShortTitle("On Quiz Ready")]
    [UnitCategory("Events\\Reflectis")]
    public class OnQuizReadyEventUnit : AwaitableEventUnit<QuizPlaceholder>
    {
        [DoNotSerialize]
        public ValueOutput Quiz { get; private set; }

        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            Quiz = ValueOutput<QuizPlaceholder>(nameof(Quiz));
        }

        // Setting the value on our port.
        protected override void AssignArguments(Flow flow, QuizPlaceholder data)
        {
            flow.SetValue(Quiz, data);
        }

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook("QuizPlaceholder" + this.ToString().Split("Unit")[0]);
        }
    }
}
