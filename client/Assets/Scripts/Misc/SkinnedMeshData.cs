using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshData : ScriptableObject
{
    public string[] bones;
    public Material[] materials;
    public Mesh mesh;

    public string[] extraTransformNames;
}
