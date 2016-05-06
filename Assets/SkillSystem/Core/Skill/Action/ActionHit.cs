using UnityEngine;
using System.Collections;
namespace Code.SkillSystem.Runtime
{
    public class ActionHit : Action
    {
        public override void Do()
        {
            base.Do();

            for (int i = 0; i < m_Summon.Targets.Count;i++ ) 
            {
                m_Summon.Targets[i].HitHandle(null);
            }
        }
    }
}