
namespace Code.SkillSystem
{
    public class MotionDirectlyTrigger : Motion
    {
        private float m_DelayTime = 0;

        public override void _Init()
        {
            base._Init();
            m_DelayTime = m_Prop.GetFloat(PropertiesKey.MOTION_DELAY_TIME);
        }

        public  override void Update(float elapsed_sec)
        {
            if (m_DelayTime <= 0)
            {
                m_Summon.Trigger(true);
            }
        }


#if UNITY_EDITOR
        public override void AddDefault(Prop prop)
        {
            base.AddDefault(prop);
            for (int i = 0; i < SkillDefaultValue.MOTION_DEFAULT_VALUE_DIRECTLYTRIGGER.Length; i++)
            {
                prop.Add(SkillDefaultValue.MOTION_DEFAULT_VALUE_DIRECTLYTRIGGER[i].key, SkillDefaultValue.MOTION_DEFAULT_VALUE_DIRECTLYTRIGGER[i].default_value);
            }
        }
#endif
    }
}