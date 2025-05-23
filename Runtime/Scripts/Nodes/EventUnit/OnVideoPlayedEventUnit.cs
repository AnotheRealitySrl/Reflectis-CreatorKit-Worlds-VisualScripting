using Reflectis.CreatorKit.Worlds.Placeholders;
using Reflectis.SDK.Core.VisualScripting;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Video: On Video Played")]
    [UnitSurtitle("BigScreen")]
    [UnitShortTitle("On Video Played")]
    [UnitCategory("Events\\Reflectis")]

    public class OnVideoPlayedEventUnit : UnityEventUnit<BigScreenPlaceholder>
    {
        protected override bool register => true;

        protected override void Definition()
        {
            base.Definition();
            //VideoPlayerReference = ValueInput<BigScreenPlaceholder>(nameof(videoPlayerReference), null).NullMeansSelf(); //this should always be self
        }

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook("BigScreen" + this.ToString().Split("EventUnit")[0]);
        }

        protected override UnityEvent GetEvent(GraphReference reference)
        {
            return reference.gameObject.GetComponent<BigScreenPlaceholder>().onVideoPlayed;
        }

        protected override BigScreenPlaceholder GetArguments(GraphReference reference)
        {
            return reference.gameObject.GetComponent<BigScreenPlaceholder>();
        }
    }
}
