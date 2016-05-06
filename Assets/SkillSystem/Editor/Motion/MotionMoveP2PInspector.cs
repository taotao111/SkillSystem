using UnityEngine;
using UnityEditor;
using Code.SkillSystem.Runtime;
namespace Code.SkillSystem.Editor
{
    [CustomEditor(typeof(MotionMoveP2P))]
    public class MotionMoveP2PInspector : MotionInspector
    {
        
        public override void OnInspectorGUI()
        {
            MotionMoveP2P motion = target as MotionMoveP2P;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(motion);
            }
        }
    }
}