using Unity.VisualScripting;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Editor
{
    public class FadeToBlackDescriptor : UnitDescriptor<FadeToBlackNode>
    {
        public FadeToBlackDescriptor(FadeToBlackNode unit) : base(unit) { }

        protected override string DefinedSummary()
        {
            return "This unit will smoothly fade the screen to black.";
        }
    }
}
