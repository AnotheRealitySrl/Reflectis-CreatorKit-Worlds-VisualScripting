using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScriptingEditor
{
    [Descriptor(typeof(TransitionProviderActivatorNode))]
    public class TransitionProviderActivatorDescriptor : UnitDescriptor<TransitionProviderActivatorNode>
    {
        public TransitionProviderActivatorDescriptor(TransitionProviderActivatorNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit requires a coroutine flow to run properly. This unit will trigger the transition of a transition provider in the " +
                "given gameobject.";
        }

    }
}
