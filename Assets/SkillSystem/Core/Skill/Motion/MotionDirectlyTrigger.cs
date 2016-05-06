
namespace Code.SkillSystem.Runtime
{
    public class MotionDirectlyTrigger : Motion
    {
        private float m_DelayTime = 0;

        public override void _Init()
        {
            base._Init();
            m_DelayTime = m_Prop.GetFloat(PropertiesKey.MOTION_DELAY_TIME);
        }

        public  override void UpdateFrame(float elapsed_sec)
        {
            if (m_DelayTime <= 0)
            {
                Summon.Trigger(true);
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