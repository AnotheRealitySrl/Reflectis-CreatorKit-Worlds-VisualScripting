using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Leaderboard Create Record: Create Record")]
    [UnitSurtitle("Leaderboard Create Record")]
    [UnitShortTitle("Create Record")]
    [UnitCategory("Reflectis\\Flow")]
    public class CreateLeaderboardRecordUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput LeaderboardKey { get; private set; }

        [NullMeansSelf]
        [DoNotSerialize]
        public ValueInput Data { get; private set; }

        protected override void Definition()
        {
            LeaderboardKey = ValueInput<string>(nameof(LeaderboardKey), string.Empty);

            Data = ValueInput<float>(nameof(Data), 0f);

            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {
                SM.GetSystem<IClientModelSystem>().CreateLeaderboardRecord(new CMLeaderboardRecord()
                {
                    LeaderboardKey = f.GetValue<string>(LeaderboardKey),
                    Data = f.GetValue<float>(Data)
                });

                return OutputTrigger;
            });

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }
    }
}
