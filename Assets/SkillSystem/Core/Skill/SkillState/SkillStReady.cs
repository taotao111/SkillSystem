using System.Collections.Generic;
using Code.StateMachine;
namespace Code.SkillSystem
{
    public class SkillStReady : State<Skill>
    {
        public bool IsMeet { get; private set; }
        private List<Condition> m_Conditions = new List<Condition>();
        public override void _Enter()
        {
            
        }

        public override void _Update(float elapsed_sec)
        {
            IsMeet = true;
            for (int i = 0; i < m_Conditions.Count; i++)
            {
                m_Conditions[i].Update(elapsed_sec);
                if (!m_Conditions[i].Check())
                {
                    IsMeet = false;
                    break;
                }
            }
        }

        public override void _Exit()
        {
            
        }
        protected override void _HandleMessage(IMessage message)
        {
            if (!IsMeet) { return; }
            base._HandleMessage(message);
            if (message is SkillMsgCast)
            {
                Switch<SkillStSing>();
            }
        }
    }
}