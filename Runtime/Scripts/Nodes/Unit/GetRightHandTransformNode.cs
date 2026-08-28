using Virtuademy.SDK.Core.Avatars;
using Virtuademy.SDK.Core.SystemFramework;
using Unity.VisualScripting;
using UnityEngine;

namespace Virtuademy.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis CMUser: Get Character Right Hand")]
    [UnitSurtitle("Character Right Hand")]
    [UnitShortTitle("Get Character Right Hand")]
    [UnitCategory("Reflectis\\Get")]
    public class GetRightHandTransformNode : Unit
    {

        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput CharacterRightHand { get; private set; }

        //private Transform _characterReference;

        protected override void Definition()
        {
            CharacterRightHand = ValueOutput<Transform>(nameof(CharacterRightHand), (flow) => SM.GetSystem<AvatarSystem>().AvatarInstance.CharacterReference.RightInteractorReference);
        }
    }
}
