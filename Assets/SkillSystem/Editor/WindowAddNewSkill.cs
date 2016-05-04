using UnityEngine;
using System.Collections;
using UnityEditor;
public class WindowAddNewSkill : EditorWindow {

    private static SkillStaticData m_data = null;
    public static void OpenWindow(SkillStaticData data)
    {
        m_data = data;
        if(m_data == null)
        {
            m_data = new SkillStaticData();
        }
        GetWindow<WindowAddNewSkill>();
    }

    void OnEnable() 
    {
        titleContent.text = "技能编辑界面";
    }
    public void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.EndVertical();
    }
}
