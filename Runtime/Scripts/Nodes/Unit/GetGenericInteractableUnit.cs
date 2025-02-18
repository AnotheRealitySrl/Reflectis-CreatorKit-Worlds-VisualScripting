using Reflectis.CreatorKit.Worlds.Core.Interaction;

using Unity.VisualScripting;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis GameObject: Get Visual Scripting Interactable")]
    [UnitSurtitle("GameObject")]
    [UnitShortTitle("Get Visual Scripting Interactable")]
    [UnitCategory("Reflectis\\Get")]
    public class GetVisualScriptingInteractableUnit : Unit
    {
        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput GameObject { get; private set; }

        [DoNotSerialize]
        public ValueOutput VisualScriptingInteractable { get; private set; }

        protected override void Definition()
        {
            GameObject = ValueInput<GameObject>(nameof(GameObject), null).NullMeansSelf();

            VisualScriptingInteractable = ValueOutput(nameof(IVisualScriptingInteractable), (flow) => flow.GetValue<GameObject>(GameObject).GetComponent<IVisualScriptingInteractable>());
        }


    }
}
