using Reflectis.CreatorKit.Worlds.Core.Interaction;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis GameObject: Get Interactable")]
    [UnitSurtitle("GameObject")]
    [UnitShortTitle("Get Interactable")]
    [UnitCategory("Reflectis\\Get")]
    public class GetInteractableUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput GameObject { get; private set; }

        [DoNotSerialize]
        public ValueOutput Interactable { get; private set; }

        protected override void Definition()
        {
            GameObject = ValueInput<GameObject>(nameof(GameObject), null).NullMeansSelf();

            Interactable = ValueOutput(nameof(Interactable), (flow) => flow.GetValue<GameObject>(GameObject).GetComponent<IInteractable>());
        }


    }
}
