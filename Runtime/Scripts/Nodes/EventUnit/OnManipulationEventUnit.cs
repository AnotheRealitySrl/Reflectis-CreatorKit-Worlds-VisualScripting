using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.CreatorKit.Worlds.Placeholders;

using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;
using static Reflectis.CreatorKit.Worlds.Core.Interaction.IManipulable;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    public abstract class OnManipulationEventUnit : EventUnit<IManipulable>
    {
        [DoNotSerialize]
        public ValueOutput Manipulable { get; private set; }
        protected override bool register => true;

        protected GraphReference graphReference;

        protected IManipulable manipulableReference;

        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            Manipulable = ValueOutput<IManipulable>(nameof(Manipulable));
        }

        protected override void AssignArguments(Flow flow, IManipulable data)
        {
            flow.SetValue(Manipulable, manipulableReference);
        }

        public override EventHook GetHook(GraphReference reference)
        {
            graphReference = reference;
            RegisterToManipulableGrab(reference.gameObject);

            return new EventHook("Manipulable" + this.ToString().Split("EventUnit")[0]);
        }

        private async void RegisterToManipulableGrab(GameObject gameObject)
        {
            manipulableReference = gameObject.GetComponent<IManipulable>();
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
            else
            {
                manipulableReference.OnCurrentStateChange.AddListener(OnManipulableStateChange);
            }
        }

        protected abstract void OnManipulableStateChange(EManipulableState manipulableState);
    }
}
