using Reflectis.CreatorKit.Worlds.Core;
using Reflectis.CreatorKit.Worlds.Core.ClientModels;
using Reflectis.CreatorKit.Worlds.Core.ObjectSpawner;
using Reflectis.SDK.Core.SystemFramework;
using Reflectis.SDK.Core.VisualScripting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Reflectis.CreatorKit.Worlds.Core.ObjectSpawner.IObjectSpawnerSystem;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis spawnable: Spawn Spawnable Object Node")]
    [UnitSurtitle("Spawnable")]
    [UnitShortTitle("SpawnSpawnableObjectNode")]
    [UnitCategory("Reflectis\\Flow")]
    public class SpawnSpawnableObjectNode : AwaitableUnit
    {
        [NullMeansSelf]
        public ValueInput SpawnableObject { get; private set; }

        public ValueInput SpawnPosition { get; private set; }


        protected override void Definition()
        {

            SpawnableObject = ValueInput<SpawnableObjectPlaceholder>(nameof(SpawnableObject), null).NullMeansSelf();
            SpawnPosition = ValueInput<Transform>(nameof(SpawnPosition), null).NullMeansSelf();

            base.Definition();

        }

        /*private ControlOutput Output(Flow flow)
        {
            return outputTrigger;
        }*/

        protected override async Task AwaitableAction(Flow flow)
        {
            SpawnableObjectPlaceholder spawnablePlaceholder = flow.GetValue<SpawnableObjectPlaceholder>(SpawnableObject);
            Transform spawnPosition = flow.GetValue<Transform>(SpawnPosition);
            Vector3 spawnPos = spawnPosition.position;
            Quaternion spawnRot = spawnPosition.rotation;

            object[] data = new object[]{
                new Dictionary<string, object>
                {
                    {
                        "GeneralContainerSpawn", new Dictionary<string, object>
                        {
                            { "spawnIndex", spawnablePlaceholder.indexSpawnReference },
                            { "listToUse", spawnablePlaceholder.listToUse }
                        }
                    }

                }
            };

            GameObject go = await SM.GetSystem<IObjectSpawnerSystem>().InstantiateObject(
                EPrefabIdentifier.GeneralContainer, spawnPos, spawnRot, SM.GetSystem<IClientModelSystem>().CurrentSession.Multiplayer, data);
        }
    }
}
