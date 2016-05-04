using UnityEngine;
using System.Collections;
using UnityEditor;

public class CustomMenuItem  {

    public const string menuRoot = "Editor Tools/";
    /// <summary>
    /// 技能编辑器
    /// </summary>
    [MenuItem(menuRoot + "Open Skill Editor(技能编辑器)",false,1000)]
    static void OpenSkillWindow()
    {
        //EditorWindow.GetWindow<WindowSkillEditor>();
        if (EditorApplication.isPlaying)
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<ISkillCaster>() != null)
            {
                EditorWindow.GetWindow<WindowSkillEditor>();
            }
            else
            {
                EditorUtility.DisplayDialog("提醒", "请选择一个角色", "确定");
            }
        }
        else
        {
            EditorApplication.isPlaying = true;

            if (EditorUtility.DisplayDialog("提醒", "请再运行状态下运行", "确定"))
            {
                //EditorWindow.GetWindow<WindowSkillEditor>();
            }
        }
    }

    [MenuItem(menuRoot + "Database/BackUp Database", false, 4000)]
    public static void BackUpDataBase()
    {
        string path = System.Environment.CurrentDirectory.Replace("\\", "/") + "/GameDB.sqlite";
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }

        System.IO.File.Copy(URL.DBPath, path);
    }

    public static void OpenProp(Prop prop)
    {
        WindowAddProp wnd = EditorWindow.GetWindow<WindowAddProp>();
        wnd.m_Prop = prop;
    }
}
