using UnityEngine;
using System.Collections;

public class LogLOD : MonoBehaviour {

    [ContextMenu("Debug LOD")]
    public void LogLODData()
    {
        LODGroup lod = gameObject.GetComponent<LODGroup>();
        if (lod != null)
        {
            foreach (var it in lod.GetLODs())
            {
                Debug.LogError(it.fadeTransitionWidth + "/" + it.screenRelativeTransitionHeight);
            }
        }
    }
}
