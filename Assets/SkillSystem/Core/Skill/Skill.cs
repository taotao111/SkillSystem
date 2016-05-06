using UnityEngine;
using System.Collections.Generic;
using Code.SkillSystem.Runtime;
using Code.StateMachine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
public interface ISkillTarget : ITarget
{
    /// <summary>
    /// 作为技能目标处理函数
    /// </summary>
    /// <param name="param">处理时需要的参数</param>
    void HandleSkill(Hashtable param);

    void HitHandle(Hashtable param);

    eCharacterLayer Layer { get; set; }
}

public interface ISkillCaster : ISkillTarget, ISummonCaster
{
    /// <summary>
    /// 获得参数
    /// </summary>
    /// <returns></returns>
    Hashtable GetParams();
}

public interface ISkill
{

}
/// <summary>
/// 技能的静态数据
/// </summary>
public class SkillStaticData : DBDataBase
{
    [DBMemberAttribute(Name = PropertiesKey.SKILL_ID, dbType = "INT")]
    public uint id;
    [DBMemberAttribute(Name = PropertiesKey.SKILL_NAME, dbType = "NVARCHAR(20)")]
    public string name;
    [DBMemberAttribute(Name = PropertiesKey.SKILL_TIME, dbType = "FLOAT")]
    public float skill_time = 1;
    [DBMemberAttribute(Name = PropertiesKey.SKILL_CD, dbType = "FLOAT")]
    public float cd = 0;
    [DBMemberAttribute(Name = PropertiesKey.SKILL_PROP, dbType = "NVARCHAR(1000)")]
    public Prop prop;

    #region 编辑器功能
#if UNITY_EDITOR
    public bool m_DrawBase = true;
    public void Draw(bool is_draw_enable)
    {
        //技能基本数据
        GUILayout.BeginVertical("box");

        m_DrawBase = EditorGUILayout.Foldout(m_DrawBase, "技能基础信息");

        if (m_DrawBase)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUI.enabled = false;
            EditorGUILayout.LabelField("id:", GUILayout.Width(200));
            id = (uint)EditorGUILayout.IntField((int)id);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUI.enabled = is_draw_enable;
            EditorGUILayout.LabelField("name:", GUILayout.Width(200));
            name = EditorGUILayout.TextField(name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.LabelField("skill_time:", GUILayout.Width(200));
            skill_time = EditorGUILayout.FloatField(skill_time);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.LabelField("cd:", GUILayout.Width(200));
            cd = EditorGUILayout.FloatField(cd);
            GUILayout.EndHorizontal();

            if (prop != null)
            {
                prop.Draw();
            }
            
        }
        GUILayout.EndVertical();
    }
#endif
    #endregion
}
/// <summary>
/// 技能动态数据
/// </summary>
public class SkillDynamicData
{
    public uint id;
    /// <summary>
    /// 当前CD
    /// </summary>
    public float cd;
    public List<Summon> summons = new List<Summon>();
    public SkillDynamicData(Skill skill)
    {
        m_Skill = skill;

        id = m_Skill.skillStaticData.id;
        cd = m_Skill.skillStaticData.cd;
    }

    private Skill m_Skill;

    public Summon BuildSummon(uint summon_id, ISkillCaster caster, ISummonCaster summon_caster,ISkillTarget summon_target,SMTargetGet trigger_target_get) 
    {
        Summon summon = new Summon();

        summon.Create(GameCenter.Instance.DataManager.skillSummonDB.Get(id,summon_id),m_Skill,caster,summon_caster, summon_target);

        summon.TriggerTargetsGet = trigger_target_get;

        summons.Add(summon);

        return summon;
    }

    public void Update(float elapsed_sec)
    {
        for (int i = 0; i < summons.Count; i++)
        {
            summons[i].Update(elapsed_sec);
        }
    }
}
public class SkillDynamicState
{
    public SkillDynamicState(Skill skill)
    {
        m_HolderSkill = skill;
    }
    private Skill m_HolderSkill;
    /// <summary>
    /// 检测状态，检查是否满足技能释放条件
    /// </summary>
    public bool IsCheck { get { return m_HolderSkill.machine.IsState<SkillStCheck>(); } }
    /// <summary>
    /// 技能准备好状态
    /// </summary>
    public bool IsReady { get { return m_HolderSkill.machine.IsState<SkillStReady>(); } }
    public bool IsSinging { get { return m_HolderSkill.machine.IsState<SkillStSing>(); } }
    public bool IsCast { get { return m_HolderSkill.machine.IsState<SkillStCast>(); } }
    public bool IsCoolDown { get { return m_HolderSkill.machine.IsState<SkillStCoolDown>(); } }
}

public class SkillTimeLine
{
    public float time;

    public void Update(float elapsed_sec)
    {
        time += elapsed_sec;
    }
}



public class Skill : ISkill, IMessageListener
{
    public ISkillCaster holder { get; private set; }
    public SkillStaticData skillStaticData { get; private set; }
    public SkillDynamicData skillDynamicData { get; private set; }
    public SkillDynamicState skillState { get; private set; }
    protected Machine<Skill> m_Machine { get; private set; }
    public TimeLine<Skill> timeLine { get; private set; }
    public Machine<Skill> machine { get { return m_Machine; } }
    public void Create(ISkillCaster caster, SkillStaticData data)
    {
        holder = caster;
        skillStaticData = data;
        skillDynamicData = new SkillDynamicData(this);

        m_Machine = new Machine<Skill>(this);
        skillState = new SkillDynamicState(this);

        timeLine = new TimeLine<Skill>(skillStaticData.id, this, skillStaticData.skill_time);
        timeLine.AddEvent(GameCenter.Instance.DataManager.timelineDB.GetTimeLineEvents(skillStaticData.id));
        //timeLine.AddEvent(new TimeEventSkillSummon((int)skillStaticData.id, 10020001, 0.5f));

        m_Machine.Register<SkillStReady>();
        m_Machine.Register<SkillStSing>();
        m_Machine.Register<SkillStCast>();
        m_Machine.Register<SkillStCoolDown>();
    }

    public void Update(float elapsed_sec)
    {
        timeLine.Update(elapsed_sec);
        m_Machine.Update(elapsed_sec);
        skillDynamicData.Update(elapsed_sec);
    }

    public void HandleMessage(IMessage message)
    {
        m_Machine.HanHandleMessage(message);
    }

    public void Release()
    {

    }

    public void AddTimeEvent(string type,Prop prop)
    {
        TimeEvent time_event = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance("TimeEvent" + type) as TimeEvent;
        time_event.Create(prop, timeLine);
        AddTimeEvent(time_event);
    }
    public void AddTimeEvent(TimeEvent time_event)
    {
        time_event.holder = timeLine;
        time_event.owner = skillStaticData.id;
        timeLine.AddEvent(time_event);
    }

    #region 编辑器脚本
#if UNITY_EDITOR
    public void ReplaceHoler(ISkillCaster caster)
    {
        holder = caster;
    }
    public void UpdateEditor(float elapsed_sec)
    {
        timeLine.Update(elapsed_sec);
        m_Machine.Update(elapsed_sec);
        skillDynamicData.Update(elapsed_sec);
    }
    public void RePlay()
    {
        timeLine.Reset();
    }
    private bool m_DrawBase = true;
    public void Draw(bool is_draw_enable)
    {
        GUILayout.BeginVertical();

        //技能基本数据
        skillStaticData.Draw(is_draw_enable);

        GUILayout.EndVertical();
    }
#endif
    #endregion
}

