using UnityEngine;
using System.Collections;
namespace Code.SkillSystem
{
    public class ActionParticle : Action
    {
        public override void Do()
        {
            base.Do();

            string ef_name = m_Prop.GetString(PropertiesKey.ACTION_EFFECT_NAME);
            string au_name = m_Prop.GetString(PropertiesKey.ACTION_AUDIO_NAME);
            bool ef_follow = m_Prop.GetBool(PropertiesKey.ACTION_EFFECT_FOLLOW, false);
            float ef_duration = m_Prop.GetFloat(PropertiesKey.ACTION_EFFECT_DURATION, -1);

            Debug.LogError("Here is play audio!!!");
            //GameCenter.Instance.AudioManager.Play(eAudioLayer.EFFECTAUDIO, au_name);

            for (int i = 0; i < m_Summon.Targets.Count; i++)
            {
                Transform mount = m_Summon.Targets[i].GetMount(m_Prop.GetString(PropertiesKey.ACTION_EFFECT_NODE));

                Effect effect = GameCenter.Instance.EffectManager.Create(ef_name,mount.position,mount.rotation,ef_duration);
                Debug.LogError("Here is play effect!!!");
                if (ef_follow)
                {
                    effect.AddComponent<FollowObjComp>();
                }
            }
        }

        #region 编辑器脚本
#if UNITY_EDITOR
        public override void AddDefault(Prop prop)
        {
            base.AddDefault(prop);
            for (int i = 0; i < SkillDefaultValue.ACTION_DEFAULT_VALUE_HP.Length; i++)
            {
                prop.Add(SkillDefaultValue.ACTION_DEFAULT_VALUE_HP[i].key, SkillDefaultValue.ACTION_DEFAULT_VALUE_HP[i].default_value);
            }
        }
#endif
        #endregion

    }
}