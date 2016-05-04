using UnityEngine;
using System.Collections;
namespace Code.SkillSystem
{
    public class ActionHp : Action
    {
        public override void Do()
        {
            base.Do();

            for (int i = 0; i < m_Summon.Targets.Count; i++ )
            {
                m_Summon.Targets[i].HandleSkill(m_Summon.skillCaster.GetParams());
            }
        }
    }
}