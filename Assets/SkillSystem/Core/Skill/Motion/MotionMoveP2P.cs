using UnityEngine;
using System.Collections;
namespace Code.SkillSystem
{
    public class MotionMoveP2P : Motion
    {
        protected Transform m_target;

        //速度
        private float m_speed;

        private Vector3 m_dir = Vector3.zero;
        

        public override void _Init()
        {
            base._Init();

            m_target = m_Summon.SummonMoveTarget.GetMount(prop.GetString(PropertiesKey.MOTION_P2P_TARGET_NODE));

            //解析prop数据
            m_speed = prop.GetFloat(PropertiesKey.MOTION_P2P_SPEED);


        }
        public override void _Update(float elapsed_sec)
        {
            base._Update(elapsed_sec);

            m_dir = m_target.position - m_Summon.Transform.position;

            float length = m_dir.magnitude;
            if (length > (m_dir.normalized * m_speed).magnitude)
            {
                m_Summon.Transform.position += m_dir.normalized * m_speed;
            }
            else
            {
                m_Summon.Transform.position = m_target.position;
                Trigger(true);
            }
        }

#if UNITY_EDITOR
        public override void AddDefault(Prop prop)
        {
            base.AddDefault(prop);
            prop.Add(PropertiesKey.MOTION_DELAY_TIME, "0");
            prop.Add(PropertiesKey.MOTION_P2P_SPEED, "1");
            prop.Add(PropertiesKey.MOTION_P2P_TARGET_NODE, "VALUE_NULL");
        }
#endif
    }
}