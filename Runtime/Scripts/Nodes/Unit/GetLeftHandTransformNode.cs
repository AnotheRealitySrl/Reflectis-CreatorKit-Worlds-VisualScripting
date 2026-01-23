using Reflectis.SDK.Core.Avatars;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;
using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis CMUser: Get Character Left Hand")]
    [UnitSurtitle("Character Left Hand")]
    [UnitShortTitle("Get Character Left Hand")]
    [UnitCategory("Reflectis\\Get")]
    public class GetLeftHandTransformNode : Unit
    {

        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput CharacterLeftHand { get; private set; }

        //private Transform _characterReference;

        protected override void Definition()
        {
            CharacterLeftHand = ValueOutput<Transform>(nameof(CharacterLeftHand), (flow) => SM.GetSystem<AvatarSystem>().AvatarInstance.CharacterReference.LeftInteractorReference);
        }
    }
}
