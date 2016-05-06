using UnityEngine;
using System.Collections;
namespace Code.SkillSystem.Runtime {
    public class SMTriggerCondition {

        protected Summon m_Summon;
        public SMTriggerCondition(Summon summon )
        { m_Summon = summon; }

        public virtual void Update(float elapsed_sec)
        {

        }

        public virtual void Trigger()
        {
            m_Summon.Trigger(true);
        }
    }
}