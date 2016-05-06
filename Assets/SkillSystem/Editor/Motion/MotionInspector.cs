using UnityEngine;
using UnityEditor;
using Code.SkillSystem.Runtime;
namespace Code.SkillSystem.Editor
{
    public class MotionInspector : UnityEditor.Editor
    {
        public bool draw_gui = true;
        public override void OnInspectorGUI()
        {
            Code.SkillSystem.Runtime.Motion motion = target as Code.SkillSystem.Runtime.Motion;
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            draw_gui = EditorGUILayout.Foldout(draw_gui, motion.prop.GetString(PropertiesKey.MOTION_TYPE));

            GUI.color = Color.red;
            if (GUILayout.Button("删除"))
            {
                motion.Summon.RemoveMotion(motion);
            }
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            if (draw_gui)
            {
                Draw();
            }

            GUILayout.EndVertical();
        }

        public virtual void Draw()
        {

        }
    }
}