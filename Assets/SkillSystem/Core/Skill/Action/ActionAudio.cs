using UnityEngine;
using System.Collections;
namespace Code.SkillSystem.Runtime
{
    public class ActionAudio : Action
    {
        public override void Do()
        {
            base.Do();
            string au_name = m_Prop.GetString(PropertiesKey.ACTION_AUDIO_NAME);
        }
    }
}