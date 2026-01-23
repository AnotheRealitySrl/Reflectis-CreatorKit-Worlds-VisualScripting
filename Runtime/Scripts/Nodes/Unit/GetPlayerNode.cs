using Reflectis.SDK.Core.Avatars;
using Reflectis.SDK.Core.SystemFramework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis CMUser: Get Character Transform")]
    [UnitSurtitle("Character Transform")]
    [UnitShortTitle("Get Character Transform")]
    [UnitCategory("Reflectis\\Get")]
    public class GetPlayerNode : Unit
    {

        [NullMeansSelf]
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput CharacterTransform { get; private set; }

        public ValueInput UserID { get; private set; }

        private List<Flow> runningFlows = new List<Flow>();

        //private Transform _characterReference;

        protected override void Definition()
        {
            UserID = ValueInput<int>(nameof(UserID));
            CharacterTransform = ValueOutput<Transform>(nameof(CharacterTransform), (flow) => SM.GetSystem<AvatarSystem>().AvatarInstance.CharacterReference.transform);
        }
    }
}
