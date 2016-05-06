using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Code.SkillSystem.Runtime
{
    public interface ISummonCaster : ITarget
    {
    }

    public class SummonData
    {
        public uint id;
        public uint owner;
        public Prop prop;

        public SummonData(Prop prop)
        {
            this.id = prop.GetUint(PropertiesKey.SUMMON_ID);
            this.owner = prop.GetUint(PropertiesKey.SUMMON_OWNER);
            this.prop = prop;
        }
    }

    public class Summon : ISummonCaster
    {
        #region PRIVATE
        private List<Motion> m_Motions = new List<Motion>();
        private List<Action> m_Actions = new List<Action>();
        private Skill m_Skill;
        private ISkillCaster m_SkillCaster;
        private ISummonCaster m_SummonCaster;
        private SummonData m_SummonData;
        private int m_TriggerCount;
        private float m_Duration;
        private ISkillTarget m_SummonMoveTarget;
        private List<ISkillTarget> m_SkillTargets = new List<ISkillTarget>();
        #endregion
        #region PUBLIC
        public bool IsExpire { get; protected set; }
        public Transform Transform { get; protected set; }
        public Effect Effect { get; private set; }
        public List<ISkillTarget> Targets { get { return m_SkillTargets; } }
        public ISkillCaster skillCaster { get { return m_SkillCaster; } }
        public ISummonCaster summonCaster { get { return m_SummonCaster; } }
        public Skill skill { get { return m_Skill; } }
        public ISkillTarget SummonMoveTarget { get { return m_SummonMoveTarget; } }
        public SMTargetGet TriggerTargetsGet { get; set; }
        #endregion

        /// <summary>
        /// 召唤物初始化
        /// </summary>
        /// <param name="summon_data"></param>
        /// <param name="skill"></param>
        /// <param name="skill_caster"></param>
        /// <param name="summon_caster"></param>
        public void Create(SummonData summon_data, Skill skill, ISkillCaster skill_caster, ISummonCaster summon_caster, ISkillTarget summon_target)
        {
            m_SummonData = summon_data;
            m_Skill = skill;
            m_SkillCaster = skill_caster;
            m_SummonCaster = summon_caster;
            m_TriggerCount = summon_data.prop.GetInt(PropertiesKey.SUMMON_TRIGGERCOUNT,0);
            m_Duration = summon_data.prop.GetInt(PropertiesKey.SUMMON_DURATION, 20);

            //要在下一行代码之前，因为在获取motion里面初始化中要使用此对象
            m_SummonMoveTarget = summon_target;
            m_Motions = GameCenter.Instance.DataManager.skillMotionDB.GetMotion(m_SummonData.owner,m_SummonData.id,this);
            m_Actions = GameCenter.Instance.DataManager.skillActionDB.GetAction(m_SummonData.owner, m_SummonData.id, this);

            GameObject summon = new GameObject(summon_data.prop.GetString(PropertiesKey.SUMMON_NAME) + "_fly");
            Transform = summon.GetComponent<Transform>();
            switch (summon_data.prop.GetString(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_TYPE))
            {
                case "summon":
                    {
                        if (m_SummonCaster != null && m_SummonCaster.GetMount(summon_data.prop.GetString(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_NAME)) != null)
                        {
                            Transform.position = m_SummonCaster.GetMount(summon_data.prop.GetString(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_NAME)).position
                                + summon_data.prop.GetVector3(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_OFFSET);
                        }
                        break;
                    }
                case "caster":
                    {
                        Transform.position = m_SkillCaster.GetMount(summon_data.prop.GetString(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_NAME)).position
                            + summon_data.prop.GetVector3(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_OFFSET);
                        break;
                    }
                case "scene":
                    {
                        Transform.position = summon_data.prop.GetVector3(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_OFFSET);
                        break;
                    }
            }

            //生成特效
            if (!string.IsNullOrEmpty(summon_data.prop.GetString(PropertiesKey.SUMMON_CREATE_EFFECT_NAME)))
            {
                Effect create_effect = GameCenter.Instance.EffectManager.Create(summon_data.prop.GetString(PropertiesKey.SUMMON_CREATE_EFFECT_NAME),Transform.position,Transform.rotation);

                create_effect.Destroy();
                //Effect.SetParent(Transform, true);
            }
            //生成音效
            GameCenter.Instance.AudioManager.Play(eAudioLayer.EFFECTAUDIO, summon_data.prop.GetString(PropertiesKey.SUMMON_CREATE_AUDIO_NAME));

            //
            if (!string.IsNullOrEmpty(summon_data.prop.GetString(PropertiesKey.SUMMON_FLY_EFFECT_NAME)))
            {
                Effect = GameCenter.Instance.EffectManager.Create(summon_data.prop.GetString(PropertiesKey.SUMMON_FLY_EFFECT_NAME), Transform.position, Transform.rotation);

                Effect.SetParent(Transform, true);
            }
        }
        public void Update(float elapsed_sec)
        {
            if(!IsExpire)
            {
                for (int i = 0; i < m_Motions.Count; i++)
                {
                    m_Motions[i].UpdateFrame(elapsed_sec);
                }
                if (m_Duration > 0)
                {
                    m_Duration -= elapsed_sec;
                }
                else
                {
                    Expire();
                }
            }
        }
        public void Trigger(bool isTrigger)
        {
            if(m_TriggerCount > 0)
            {
                for (int i = 0; i < m_Actions.Count; i++)
                {
                    m_Actions[i].Do();
                }
                m_TriggerCount--;
                if(m_TriggerCount <= 0)
                {
                    Expire();
                }
            }

        }
        private void TriggerEffect()
        {
            string name = m_SummonData.prop.GetString(PropertiesKey.SUMMON_TRIGGER_EFFECT_NAME);
            //生成特效
            if (!string.IsNullOrEmpty(name))
            {
                Effect create_effect = GameCenter.Instance.EffectManager.Create(name, Transform.position  + m_SummonData.prop.GetVector3(PropertiesKey.SUMMON_TRIGGER_EFFECT_NODE_OFFSET), Transform.rotation);

                create_effect.Destroy();
                //Effect.SetParent(Transform, true);
            }
            //生成音效
            name = m_SummonData.prop.GetString(PropertiesKey.SUMMON_TRIGGER_AUDIO_NAME);
            GameCenter.Instance.AudioManager.Play(eAudioLayer.EFFECTAUDIO, m_SummonData.prop.GetString(PropertiesKey.SUMMON_TRIGGER_AUDIO_NAME));
        }
        public void Expire()
        {
            IsExpire = true;
        }
        public UnityEngine.Transform GetMount(string name)
        {
            return this.Transform;
        }
        public void AddTarget(ISkillTarget target)
        {
            m_SkillTargets.Add(target);
        }
        public void RemoveTarget(ISkillTarget target)
        {
            m_SkillTargets.Remove(target);
        }
        
        #region 编辑器脚本
#if UNITY_EDITOR
        private bool m_DrawMotion = true;
        private bool m_DrawAction = true;

        private eMotion m_CreateMotionType = eMotion.M_DirectlyTrigger;
        private eAction m_CreateActionType = eAction.Hp;
        public void LoadData(ISkillTarget target, uint skill_id,uint summon_id)
        {
            m_SummonMoveTarget = target;

            m_SummonData = GameCenter.Instance.DataManager.skillSummonDB.Get(skill_id, summon_id);

            //load motion and action
            m_Motions = GameCenter.Instance.DataManager.skillMotionDB.GetMotion(skill_id, summon_id, this);
            m_Actions = GameCenter.Instance.DataManager.skillActionDB.GetAction(m_SummonData.owner, m_SummonData.id, this);


            DrawStyleInt summon_id_draw = DrawStyle.IntStyle(PropertiesKey.SUMMON_ID);
            summon_id_draw.enable = false;
            m_SummonData.prop.AddStyle(summon_id_draw);

            DrawStyleInt summon_owner_draw = DrawStyle.IntStyle(PropertiesKey.SUMMON_OWNER);
            summon_owner_draw.enable = false;
            m_SummonData.prop.AddStyle(summon_owner_draw);
        }
        public void Remove(uint skill_id, uint summon_id)
        {
            m_SummonData = GameCenter.Instance.DataManager.skillSummonDB.Get(skill_id, summon_id);
            GameCenter.Instance.DataManager.skillMotionDB.Remove(skill_id, summon_id);
            GameCenter.Instance.DataManager.skillActionDB.Remove(m_SummonData.owner, m_SummonData.id);

            GameCenter.Instance.DataManager.skillSummonDB.Remove(m_SummonData);
        }

        public void Draw()
        {
            GUILayout.BeginVertical("box");

            m_SummonData.prop.Draw();
            #region Motion
            GUILayout.BeginVertical("box");
            m_DrawMotion = EditorGUILayout.Foldout(m_DrawMotion,"Motions");

            if (m_DrawMotion)
            {
                for (int i = 0; i < m_Motions.Count; i++)
                {
                    m_Motions[i].Draw();
                }

                EditorGUILayout.Space();
                GUILayout.BeginHorizontal("box");

                m_CreateMotionType = (eMotion)EditorGUILayout.EnumPopup(m_CreateMotionType);
                if (GUILayout.Button("添加Motion"))
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly(); // 获取当前程序集 

                    Motion motion = (Motion)assembly.CreateInstance("Code.SkillSystem.Runtime." + m_CreateMotionType.ToString().Replace("M_", "Motion"));

                    if (motion != null)
                    {
                        motion.AddDefault(motion.prop);
                        motion.Create_Editor(motion.prop, this);

                        motion.prop.SetValue(PropertiesKey.MOTION_OWNER_SKILL, m_SummonData.owner.ToString());
                        motion.prop.SetValue(PropertiesKey.MOTION_OWNER, m_SummonData.id.ToString());
                        motion.prop.SetValue(PropertiesKey.SUMMON_ID, (GameCenter.Instance.DataManager.skillMotionDB.MaxID(motion.prop) + 1).ToString());
                        m_Motions.Add(motion);
                        GameCenter.Instance.DataManager.skillMotionDB.Add(motion.prop);
                    }
                }
                GUILayout.EndHorizontal();

            }
            GUILayout.EndVertical();
            #endregion

            #region Action
            GUILayout.BeginVertical("box");
            m_DrawAction = EditorGUILayout.Foldout(m_DrawAction, "Actions");

            if (m_DrawAction)
            {
                for (int i = 0; i < m_Actions.Count; i++)
                {
                    m_Actions[i].Draw();
                }

                GUILayout.BeginHorizontal("box");
                m_CreateActionType = (eAction)EditorGUILayout.EnumPopup(m_CreateActionType);
                if (GUILayout.Button("添加Action"))
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly(); // 获取当前程序集 


                    Action action = (Action)assembly.CreateInstance("Code.SkillSystem.Runtime.Action" + m_CreateActionType.ToString());

                    if (action != null)
                    {
                        action.AddDefault(action.prop);
                        action.Create(action.prop, this);
                        action.prop.SetValue(PropertiesKey.ACTION_OWNER_SKILL, m_SummonData.owner.ToString());
                        action.prop.SetValue(PropertiesKey.ACTION_OWNER, m_SummonData.id.ToString());
                        action.prop.SetValue(PropertiesKey.ACTION_ID, (GameCenter.Instance.DataManager.skillActionDB.MaxID(action.prop) + 1).ToString());
                        m_Actions.Add(action);
                        GameCenter.Instance.DataManager.skillActionDB.Add(action.prop);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            #endregion

            GUILayout.EndVertical();
        }
        public void RemoveMotion(Motion motion)
        {
            m_Motions.Remove(motion);
            GameCenter.Instance.DataManager.skillMotionDB.Remove(motion.prop);
        }
        public void RemoveAction(Action action)
        {
            m_Actions.Remove(action);
            GameCenter.Instance.DataManager.skillActionDB.Remove(action.prop);
        }
        public void Save()
        {
            

        }
#endif
        #endregion
    }
}