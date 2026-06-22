using UnityEngine;

[DisallowMultipleComponent]
public class MeshKeeper : MonoBehaviour
{
    // This permanently holds the reference to your untouched FBX asset
    [HideInInspector]
    public Mesh originalMesh;
}