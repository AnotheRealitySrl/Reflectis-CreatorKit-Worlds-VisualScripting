using Reflectis.SDK.Core.VisualScripting;

using System.Threading.Tasks;

using Unity.VisualScripting;
using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Platform: Load Default Event")]
    [UnitSurtitle("Platform")]
    [UnitShortTitle("Load Default Event")]
    [UnitCategory("Reflectis\\Flow")]
    public class LoadDefaultEventNode : AwaitableUnit
    {
        protected override async Task AwaitableAction(Flow flow)
        {
            Debug.LogWarning($"[LoadDefaultEventNode] Default events are deprecated. The execution of this node will be ignored.");
            //await IReflectisApplicationManager.Instance.LoadDefaultEvent();
        }
    }
}
