using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Code.SkillSystem.Runtime
{
    public class TimeEventSkillSummon : TimeEvent
    {
        [TimeEventAttributes(Name = PropertiesKey.TIMELINE_SUMMON_ID, dbType = "INT")]
        public int summon_id;
        [TimeEventAttributes(Name = PropertiesKey.TIMELINE_SUMMON_TARGET, dbType = "VARCHAR(1000)", ConverterType = typeof(ConvertStringToProp))]
        public Prop sm_target_prop;
        [TimeEventAttributes(Name = PropertiesKey.TIMELINE_SUMMON_TRIGGER_TARGET, dbType = "VARCHAR(1000)", ConverterType = typeof(ConvertStringToProp))]
        public Prop sm_trigger_prop;

        public TimeEventSkillSummon()
            : base()
        {
            eventType = TimeEventType.TimeEventSkillSummon;
        }

        public override void Trigger()
        {
            base.Trigger();
            TimeLine<Skill> line = (TimeLine<Skill>)holder;

            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 

            SMTargetGet sm_target = assembly.CreateInstance("Code.SkillSystem.Runtime." + sm_target_prop.GetString(PropertiesKey.TIMELINE_SUMMON_TARGET), true, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { ((Skill)holder.GetHolder()).holder }, null, null) as Code.SkillSystem.Runtime.SMTargetGet;

            System.Collections.Generic.List<ISkillTarget> targets = sm_target.Get();

            for (int i = 0; i < targets.Count; i++)
            {
                if (sm_trigger_prop != null)
                {
                    SMTargetGet sm_trigger_target_get = assembly.CreateInstance("Code.SkillSystem.Runtime." + sm_target_prop.GetString(PropertiesKey.TIMELINE_SUMMON_TARGET), true, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { ((Skill)holder.GetHolder()).holder }, null, null) as Code.SkillSystem.Runtime.SMTargetGet;

                    line.holder.skillDynamicData.BuildSummon((uint)summon_id, line.holder.holder, null, targets[i], sm_trigger_target_get);
                }
                else
                {
                    line.holder.skillDynamicData.BuildSummon((uint)summon_id, line.holder.holder, null, targets[i],null);
                }

            }
        }

        #region 编辑器脚本
#if UNITY_EDITOR
        public eSMTarget d_sm_type = eSMTarget.SMTargetGetNear;

        public override void Draw()
        {
            base.Draw();
            GUILayout.BeginVertical("box");

            //技能基本数据
            if (m_IsDraw)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.LabelField("summon_id:", GUILayout.Width(200));
                summon_id = EditorGUILayout.IntField(summon_id);

                if (GUILayout.Button("编辑"))
                {
                    SkillProp.right_draw = SkillProp.DrawSkillType.Summon;
                    if (SkillProp.CreateSummon != null)
                    {
                        TimeLine<Skill> line = (TimeLine<Skill>)holder;
                        SkillProp.CreateSummon(line.holder.skillStaticData.id, (uint)summon_id);
                    }
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("box");

                if (sm_target_prop == null)
                {
                    GUILayout.BeginHorizontal();

                    d_sm_type = (eSMTarget)EditorGUILayout.EnumPopup(d_sm_type);

                    if (GUILayout.Button("添加"))
                    {
                        sm_target_prop = new Prop();
                        sm_target_prop.Add(PropertiesKey.TIMELINE_SUMMON_TARGET, d_sm_type.ToString());

                        //add default value
                        Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 

                        Code.SkillSystem.Runtime.SMTargetGet sm_target = assembly.CreateInstance("Code.SkillSystem.Runtime." + sm_target_prop.GetString(PropertiesKey.TIMELINE_SUMMON_TARGET), true, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { ((Skill)holder.GetHolder()).holder }, null, null) as Code.SkillSystem.Runtime.SMTargetGet;

                        sm_target.AddDefault(sm_target_prop);
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    sm_target_prop.Draw("飞行物飞行目标");

                    GUILayout.BeginHorizontal();
                    d_sm_type = (eSMTarget)EditorGUILayout.EnumPopup(d_sm_type);

                    if (GUILayout.Button("更换"))
                    {
                        sm_target_prop = new Prop();
                        sm_target_prop.Add(PropertiesKey.TIMELINE_SUMMON_TARGET, d_sm_type.ToString());

                        //add default value
                        Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 

                        Code.SkillSystem.Runtime.SMTargetGet sm_target = assembly.CreateInstance("Code.SkillSystem.Runtime." + sm_target_prop.GetString(PropertiesKey.TIMELINE_SUMMON_TARGET), true, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { ((Skill)holder.GetHolder()).holder }, null, null) as Code.SkillSystem.Runtime.SMTargetGet;

                        sm_target.AddDefault(sm_target_prop);
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();

            }

            GUILayout.EndVertical();
        }
        public override void Remove()
        {
            if (SkillProp.RemoveSummon != null)
            {
                TimeLine<Skill> line = (TimeLine<Skill>)holder;
                SkillProp.RemoveSummon(line.holder.skillStaticData.id, (uint)summon_id);
            }
            base.Remove();
        }
#endif
        #endregion
    }
}