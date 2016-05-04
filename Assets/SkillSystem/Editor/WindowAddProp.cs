using UnityEngine;
using System.Collections;
using UnityEditor;
public class WindowAddProp : EditorWindow {
    
    private string m_Prop_Name = string.Empty;
    private string m_Prop_Value = string.Empty;
    public Prop m_Prop;

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("name:", GUILayout.Width(40));
        m_Prop_Name = EditorGUILayout.TextArea(m_Prop_Name);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("value:", GUILayout.Width(48));
        m_Prop_Value = EditorGUILayout.TextArea(m_Prop_Value);
        GUILayout.EndHorizontal();

        if(GUILayout.Button("添加"))
        {
            if (string.IsNullOrEmpty(m_Prop_Name))
            {
                EditorUtility.DisplayDialog("注意", "属性名为空，请填写属性名！！！", "确定");
                return;
            }
            if (string.IsNullOrEmpty(m_Prop_Value))
            {
                EditorUtility.DisplayDialog("注意", "值为空，请填写值！！！", "确定");
                return;
            }
            if(m_Prop != null)
            {
                if (!m_Prop.HasValue(m_Prop_Name))
                {
                    m_Prop.Add(m_Prop_Name, m_Prop_Value);
                    if (EditorUtility.DisplayDialog("注意", "添加成功", "确定"))
                    {
                        Close();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("注意", "已包含该名字!!!!", "确定");
                }

            }
        }
    }
}
