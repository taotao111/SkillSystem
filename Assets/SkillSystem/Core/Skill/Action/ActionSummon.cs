using UnityEngine;
using System.Collections;
namespace Code.SkillSystem
{
    public class ActionSummon : Action
    {
        public override void Do()
        {
            base.Do();

            uint summon_id = m_Prop.GetUint(PropertiesKey.ACTION_SUMMON_ID);

            //m_Summon.skill.skillDynamicData.BuildSummon(summon_id, m_Summon.skill.holder, m_Summon);
        }
    }
}