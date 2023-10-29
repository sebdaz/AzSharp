using UnityEngine;

namespace AzSharp.ECS.Unity.Mono;

public class ECSPrototype : MonoBehaviour
{
    /// The name of the prefab that will be created.
    public string PrototypeName = string.Empty;
    public string ExtraComponent = string.Empty;
    public bool InheritTransform = true;
}
