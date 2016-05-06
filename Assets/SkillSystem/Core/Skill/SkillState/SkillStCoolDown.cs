using Code.StateMachine;
namespace Code.SkillSystem.Runtime
{
    public class SkillStCoolDown : State<Skill>
    {
        private float m_CD = 0;
        private float m_CurrectCD = 0;
        protected override void _Init()
        {
            base._Init();
            m_CD = Holder.skillStaticData.cd;
        }
        public override void _Enter()
        {
            m_CurrectCD = m_CD;
        }

        public override void _Update(float elapsed_sec)
        {
            m_CurrectCD -= elapsed_sec;

            Holder.skillDynamicData.cd = m_CurrectCD;

            if (m_CurrectCD <= 0)
            {
                Switch<SkillStCheck>();
            }

        }

        public override void _Exit()
        {
            
        }
    }
}