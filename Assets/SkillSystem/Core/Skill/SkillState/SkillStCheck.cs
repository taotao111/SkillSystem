using System.Collections.Generic;
using Code.StateMachine;
namespace Code.SkillSystem
{
    public class SkillStCheck : State<Skill>
    {
        private List<Condition> m_Conditions = new List<Condition>();
        public override void _Enter()
        {
            
        }

        public override void _Update(float elapsed_sec)
        {
            bool to_next = true;
            for (int i = 0; i < m_Conditions.Count; i++ )
            {
                m_Conditions[i].Update(elapsed_sec);
                if(!m_Conditions[i].Check())
                {
                    to_next = false;
                    break;
                }
            }

            if(to_next)
            {
                Switch<SkillStReady>();
            }
        }

        public override void _Exit()
        {
            
        }
    }
}