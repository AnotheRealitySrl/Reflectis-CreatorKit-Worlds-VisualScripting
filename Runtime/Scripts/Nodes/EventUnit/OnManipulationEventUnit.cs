using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.CreatorKit.Worlds.Placeholders;
using Reflectis.SDK.Core.VisualScripting;
using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Events;
using static Reflectis.CreatorKit.Worlds.Core.Interaction.IInteractable;
using static Reflectis.CreatorKit.Worlds.Core.Interaction.IManipulable;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    public abstract class OnManipulationEventUnit : UnityEventUnit<IManipulable, EManipulableState>
    {
        [DoNotSerialize]
        public ValueOutput Manipulable { get; private set; }
        protected override bool register => true;

        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            Manipulable = ValueOutput<IManipulable>(nameof(Manipulable));
        }

        protected override void AssignArguments(Flow flow, IManipulable data)
        {
            flow.SetValue(Manipulable, data);
        }

        public override async void Instantiate(GraphReference instance)
        {
            await AwaitManipulableSetup(instance.gameObject);

            base.Instantiate(instance);
        }

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook("Manipulable" + this.ToString().Split("EventUnit")[0]);
        }

        private async Task AwaitManipulableSetup(GameObject gameObject)
        {
            var manipulableReference = gameObject.GetComponent<IManipulable>();
            if (manipulableReference == null)
            {
                if (gameObject.TryGetComponent<ManipulablePlaceholder>(out var _))
                {
                    manipulableReference = gameObject.GetComponent<IManipulable>();
                    while (manipulableReference == null)
                    {
                        await Task.Yield();
                        manipulableReference = gameObject.GetComponent<IManipulable>();
                    }
                }
            }
            if (manipulableReference == null)
            {
                Debug.LogError("The unit " + this.ToString().Split("EventUnit")[0] + "in the graph on the game object " + gameObject + " needs a proper interactable placeholder" +
                    " on the game object to function.");
            }
        }

        protected override UnityEvent<EManipulableState> GetEvent(GraphReference reference)
        {
            var manipulableReference = reference.gameObject.GetComponent<IManipulable>();
            if (manipulableReference == null)
            {
                return new UnityEvent<EManipulableState>();
            }
            return manipulableReference.OnCurrentStateChange;
        }

        protected override IManipulable GetArguments(GraphReference reference, EManipulableState state)
        {
            return reference.gameObject.GetComponent<IManipulable>();
        }

        protected override UnityAction<EManipulableState> GetData(GraphReference reference)
        {
            return (x) =>
            {
                if (ShouldTriggerOnChange(x))
                {
                    Trigger(reference, GetArguments(reference, x));
                }
            };
        }

        protected abstract bool ShouldTriggerOnChange(EManipulableState manipulableState);
    }
}
