using Code.StateMachine;
namespace Code.SkillSystem.Runtime
{
    public class SkillStSing : State<Skill>
    {
        private float m_WaitTime = 0;
        public override void _Enter()
        {
            
        }

        public override void _Update(float elapsed_sec)
        {
            m_WaitTime -= elapsed_sec;
            if(m_WaitTime <= 0)
            {
                Switch<SkillStCast>();
            }
        }

        public override void _Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}