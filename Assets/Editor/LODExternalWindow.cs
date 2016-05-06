using UnityEngine;
using System.Collections;
using UnityEditor;
public class LODExternalWindow : EditorWindow {

    [MenuItem("Tools/LOD/Open lod windows")]
    public static void OpenLODExternalWindow()
    {
        EditorWindow.GetWindow<LODExternalWindow>();
    }
    float value = 0;

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey("LOD Value"))
        {
            PlayerPrefs.SetFloat("LOD Value", 0.1f);
        }

        value = PlayerPrefs.GetFloat("LOD Value");
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        value = EditorGUILayout.Slider(value, 0, 1);
        PlayerPrefs.SetFloat("LOD Value", value);
        if (GUILayout.Button("设置"))
        {
            foreach (var it in Selection.gameObjects)
            {
                LODGroup lod = it.GetComponent<LODGroup>();
                if (lod != null)
                {
                    LOD[] lods = lod.GetLODs();

                    if (lods.Length >= 2)
                    {
                        lods[1].screenRelativeTransitionHeight = value;
                    }

                    lod.SetLODs(lods);
                }
            }
        }
        GUILayout.EndHorizontal();
    }
}
