using UnityEngine;
using System.Collections;
namespace Code.SkillSystem.Runtime
{
    public enum eMotion
    {
        M_DirectlyTrigger,//直接触发
        MotionMoveP2P,//
    }

    public class MotionBuilder : TBuilder<Motion, Summon>
    {
        public MotionBuilder()
        {
            
        }

        void Add<T>(eMotion id) where T : Motion, new()
        {
            Add<T>((uint)id);
        }
    }
}