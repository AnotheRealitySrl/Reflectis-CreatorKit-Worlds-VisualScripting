using Virtuademy.CreatorKit.Worlds.Core;
using Virtuademy.CreatorKit.Worlds.Core.ClientModels;
using Virtuademy.CreatorKit.Worlds.Core.ObjectSpawner;
using Virtuademy.SDK.Core.SystemFramework;
using Virtuademy.SDK.Core.VisualScripting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Virtuademy.CreatorKit.Worlds.Core.ObjectSpawner.IObjectSpawnerSystem;

namespace Virtuademy.CreatorKit.Worlds.VisualScripting
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
