using Reflectis.SDK.Core.Avatars;
using Reflectis.SDK.Core.SystemFramework;
using Unity.VisualScripting;
using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis CMUser: Get Character Head Transform")]
    [UnitSurtitle("Character Head Transform")]
    [UnitShortTitle("Get Character Head Transform")]
    [UnitCategory("Reflectis\\Get")]
    public class GetCharacterHeadNode : Unit
    {

        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput CharacterHeadReference { get; private set; }

        protected override void Definition()
        {
            CharacterHeadReference = ValueOutput<Transform>(nameof(CharacterHeadReference), (flow) => SM.GetSystem<AvatarSystem>().AvatarInstance.CharacterReference.HeadReference);
        }
    }
}
