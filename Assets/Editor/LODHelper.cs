using UnityEngine;
using System.Collections;
using UnityEditor;
public class LODHelper
{
    [MenuItem("Tools/LOD/Add LOD")]
    public static void AddLOD()
    {
        foreach (var it in Selection.gameObjects)
        {
            LODGroup lod = it.GetComponent<LODGroup>();
            if (lod == null)
            {
                lod = it.AddComponent<LODGroup>();
            }

            lod.SetLODs(new LOD[] {new LOD(1,it.GetComponentsInChildren<Renderer>()),new LOD(0.1f, it.GetComponentsInChildren<Renderer>()) });
        }
    }
}
