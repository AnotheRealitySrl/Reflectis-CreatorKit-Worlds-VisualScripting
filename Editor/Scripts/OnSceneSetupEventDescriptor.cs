using Reflectis.CreatorKit.Worlds.VisualScripting;
using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScriptingEditor
{
    [Descriptor(typeof(OnSceneSetupEventNode))]
    public class OnSceneSetupEventDescriptor : UnitDescriptor<OnSceneSetupEventNode>
    {
        public OnSceneSetupEventDescriptor(OnSceneSetupEventNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This event unit will be triggered when the scene has been setup, after all the placeholders" +
                " have been mapped. The scene loading screen will await the end of every running flow launched" +
                " by this event before disappearing.";
        }
    }
}
