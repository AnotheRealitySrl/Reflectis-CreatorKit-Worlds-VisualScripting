using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    public abstract class SyncedVariableBaseEventUnit<T> : EventUnit<T>
    {

        [DoNotSerialize]
        public ValueInput VariableName { get; private set; }

        protected override void Definition()
        {
            base.Definition();
            VariableName = ValueInput<string>(nameof(VariableName), null);
        }
    }
}
