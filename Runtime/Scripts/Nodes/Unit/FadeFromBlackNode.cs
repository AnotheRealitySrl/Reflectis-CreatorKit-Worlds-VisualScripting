using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.Fade;

using System.Collections;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Scene: Fade From Black")]
    [UnitSurtitle("Scene")]
    [UnitShortTitle("Fade From Black")]
    [UnitCategory("Reflectis\\Flow")]
    public class FadeFromBlackNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        protected override void Definition()
        {
            InputTrigger = ControlInputCoroutine(nameof(InputTrigger), FadeFromBlackCoroutine);

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }

        private IEnumerator FadeFromBlackCoroutine(Flow flow)
        {
            bool fadeDone = false;

            SM.GetSystem<IFadeSystem>().FadeFromBlack(() => fadeDone = true);

            yield return new WaitUntil(() => fadeDone == true);

            yield return OutputTrigger;
        }
    }
}
