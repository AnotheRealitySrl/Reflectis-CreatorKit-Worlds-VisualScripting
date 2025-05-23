using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.Fade;

using System.Collections;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Scene: Fade To Black")]
    [UnitSurtitle("Scene")]
    [UnitShortTitle("Fade To Black")]
    [UnitCategory("Reflectis\\Flow")]
    public class FadeToBlackNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        protected override void Definition()
        {
            InputTrigger = ControlInputCoroutine(nameof(InputTrigger), FadeToBlackCoroutine);

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }

        private IEnumerator FadeToBlackCoroutine(Flow flow)
        {
            bool fadeDone = false;

            SM.GetSystem<IFadeSystem>().FadeToBlack(() => fadeDone = true);

            yield return new WaitUntil(() => fadeDone == true);

            yield return OutputTrigger;
        }
    }
}
