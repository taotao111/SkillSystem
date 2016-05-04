using UnityEngine;
using System.Collections;

public class PropertiesKey {

    public const string ID = "id";

    #region SKILL
    /// <summary>
    /// 技能ID关键字
    /// </summary>
    public const string SKILL_ID = "id";
    /// <summary>
    /// 技能名
    /// </summary>
    public const string SKILL_NAME = "name";
    /// <summary>
    /// 技能时长
    /// </summary>
    public const string SKILL_TIME = "skill_time";
    /// <summary>
    /// 技能cd
    /// </summary>
    public const string SKILL_CD = "cd";
    /// <summary>
    /// 技能属性
    /// </summary>
    public const string SKILL_PROP = "prop";
    #endregion

    #region SUMMON
    /// <summary>
    /// 飞行物id
    /// </summary>
    public const string SUMMON_ID = "id";
    /// <summary>
    /// 飞行物属于哪个技能id
    /// </summary>
    public const string SUMMON_OWNER = "owner";
    /// <summary>
    /// 飞行物名字
    /// </summary>
    public const string SUMMON_NAME = "name";
    /// <summary>
    /// 触发次数
    /// </summary>
    public const string SUMMON_TRIGGERCOUNT = "trigger_count";
    /// <summary>
    /// 持续时间
    /// </summary>
    public const string SUMMON_DURATION = "duration";
    /// <summary>
    /// 飞行物属性
    /// </summary>
    public const string SUMMON_PROP = "prop";

    #region SUMMON_EFFECT_NODE_TYPE 枚举
    /// <summary>
    /// 节点类型：施法者
    /// </summary>
    public const string SUMMON_EFFECT_NODE_TYPE_CASTER = "caster";
    /// <summary>
    /// 节点类型：场景
    /// </summary>
    public const string SUMMON_EFFECT_NODE_TYPE_SCENE = "scene";
    /// <summary>
    /// 节点类型：飞行物
    /// </summary>
    public const string SUMMON_EFFECT_NODE_TYPE_SUMMON = "summon";
    #endregion

    /// <summary>
    /// 飞行物特效名字
    /// </summary>
    public const string SUMMON_CREATE_EFFECT_NAME = "create_ef_name";
    /// <summary>
    /// 飞行物绑定节点类型
    /// </summary>
    public const string SUMMON_CREATE_EFFECT_NODE_TYPE = "create_ef_node_type";
    /// <summary>
    /// 飞行物节点名字
    /// </summary>
    public const string SUMMON_CREATE_EFFECT_NODE_NAME = "create_ef_node_name";
    /// <summary>
    /// 飞行物节点偏移
    /// </summary>
    public const string SUMMON_CREATE_EFFECT_NODE_OFFSET = "create_ef_node_offset";
    /// <summary>
    /// 飞行物音效名字
    /// </summary>
    public const string SUMMON_CREATE_AUDIO_NAME = "create_au_name";

    public const string SUMMON_FLY_EFFECT_NAME = "fly_ef_name";

    /// <summary>
    /// 飞行物触发特效名字
    /// </summary>
    public const string SUMMON_TRIGGER_EFFECT_NAME = "trigger_ef_name";
    /// <summary>
    /// 飞行物触发节点偏移
    /// </summary>
    public const string SUMMON_TRIGGER_EFFECT_NODE_OFFSET = "trigger_ef_node_offset";
    /// <summary>
    /// 飞行物触发音效名字
    /// </summary>
    public const string SUMMON_TRIGGER_AUDIO_NAME = "trigger_au_name";
    #endregion

    #region SUMMON TARGET
    /// <summary>
    /// 飞行物目标属性
    /// </summary>
    public const string SUMMONTARGET_PROP = "prop";
    #endregion

    #region MOTION
    /// <summary>
    /// 运动id
    /// </summary>
    public const string MOTION_ID = "id";
    /// <summary>
    /// 运动属于飞行物id
    /// </summary>
    public const string MOTION_OWNER = "owner";
    /// <summary>
    /// 运动属于技能id
    /// </summary>
    public const string MOTION_OWNER_SKILL = "owner_skill";
    /// <summary>
    /// 运动类型
    /// </summary>
    public const string MOTION_TYPE = "motion_type";
    /// <summary>
    /// 运动自动消亡时间
    /// </summary>
    public const string MOTION_AUTO_DESTROY_TIME = "auto_destory_time";
    /// <summary>
    /// 运动延时时间
    /// </summary>
    public const string MOTION_DELAY_TIME = "delay_time";
    /// <summary>
    /// 用于运动计算的属性
    /// </summary>
    public const string MOTION_PROP = "prop";

    #region MOTION MOVEP2P
    /// <summary>
    /// p2p运动是的速度
    /// </summary>
    public static string MOTION_P2P_SPEED = "move_speed";
    /// <summary>
    /// p2p目标节点
    /// </summary>
    public const string MOTION_P2P_TARGET_NODE = "target_node";
    #endregion

    #endregion

    #region ACTION
    /// <summary>
    /// 行为id
    /// </summary>
    public const string ACTION_ID = "id";
    /// <summary>
    /// 行为类型
    /// </summary>
    public const string ACTION_TYPE = "action_type";
    /// <summary>
    /// 行为属于哪一个飞行物
    /// </summary>
    public const string ACTION_OWNER = "owner";
    public const string ACTION_OWNER_SKILL = "owner_skill";

    /// <summary>
    /// 创建的下一个飞行物ID
    /// </summary>
    public const string ACTION_SUMMON_ID = "summon_id";
    /// <summary>
    /// 行为效果是否跟随
    /// </summary>
    public const string ACTION_EFFECT_FOLLOW = "ef_follow";
    /// <summary>
    /// 行为效果的持续时间
    /// </summary>
    public const string ACTION_EFFECT_DURATION = "ef_duration";
    /// <summary>
    /// 行为效果名字
    /// </summary>
    public const string ACTION_EFFECT_NAME = "ef_name";
    /// <summary>
    /// 行为效果绑定节点
    /// </summary>
    public const string ACTION_EFFECT_NODE = "ef_node";
    /// <summary>
    /// 行为效果音效名字
    /// </summary>
    public const string ACTION_AUDIO_NAME = "au_name";
    #endregion

    #region TIMELINE
    /// <summary>
    /// 时间事件id
    /// </summary>
    public const string TIMELINE_ID = "id";
    /// <summary>
    /// 时间事件属于的id
    /// </summary>
    public const string TIMELINE_OWNER = "owner";
    /// <summary>
    /// 创建飞行物事件的飞行物id
    /// </summary>
    public const string TIMELINE_SUMMON_ID = "summon_id";
    /// <summary>
    /// 创建飞行物事件的目标
    /// </summary>
    public const string TIMELINE_SUMMON_TARGET = "sm_target";
    /// <summary>
    /// 事件的时间
    /// </summary>
    public const string TIMELINE_TIME = "time";
    /// <summary>
    /// 事件的属性
    /// </summary>
    public const string TIMELINE_PROP = "prop";
    /// <summary>
    /// 事件类型
    /// </summary>
    public const string TIMELINE_EVENT_TYPE = "event_type";
    #endregion


}
