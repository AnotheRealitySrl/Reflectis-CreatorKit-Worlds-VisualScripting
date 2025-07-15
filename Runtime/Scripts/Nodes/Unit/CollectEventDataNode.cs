using Reflectis.CreatorKit.Worlds.Core.ClientModels;

using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Expose: CMEvent")]
    [UnitSurtitle("Expose")]
    [UnitShortTitle("CMEvent")]
    [UnitCategory("Reflectis\\Expose")]
    public class CollectEventDataNode : Unit
    {
        [DoNotSerialize]
        public ValueInput CMEvent { get; private set; }

        [DoNotSerialize]
        public ValueOutput ID { get; private set; }
        [DoNotSerialize]
        public ValueOutput Title { get; private set; }
        [DoNotSerialize]
        public ValueOutput Description { get; private set; }
        [DoNotSerialize]
        public ValueOutput StartDateTime { get; private set; }
        [DoNotSerialize]
        public ValueOutput EndDateTime { get; private set; }
        [DoNotSerialize]
        public ValueOutput Tags { get; private set; }
        [DoNotSerialize]
        public ValueOutput IsEventPublic { get; private set; }
        [DoNotSerialize]
        public ValueOutput IsEventStatic { get; private set; }

        protected override void Definition()
        {
            CMEvent = ValueInput<CMSession>(nameof(CMEvent), null).NullMeansSelf();

            ID = ValueOutput(nameof(ID), (flow) => flow.GetValue<CMSession>(CMEvent).Id);

            Title = ValueOutput(nameof(Title), (flow) => flow.GetValue<CMSession>(CMEvent).Experience.Title);

            Description = ValueOutput(nameof(Description), (flow) => flow.GetValue<CMSession>(CMEvent).Experience.Description);

            StartDateTime = ValueOutput(nameof(StartDateTime), (flow) => flow.GetValue<CMSession>(CMEvent).StartDateTime);

            EndDateTime = ValueOutput(nameof(EndDateTime), (flow) => flow.GetValue<CMSession>(CMEvent).EndDateTime);

            Tags = ValueOutput(nameof(Tags), (flow) => flow.GetValue<CMSession>(CMEvent).Tags);

            IsEventPublic = ValueOutput(nameof(IsEventPublic), (flow) => flow.GetValue<CMSession>(CMEvent).IsPublic);
        }
    }
}