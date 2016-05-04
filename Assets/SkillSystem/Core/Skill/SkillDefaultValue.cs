using UnityEngine;
using System.Collections;

public class SkillDefaultValue
{
    public const string DEFAULTVALUE_STRING = "DEFAULT_VALUE";
    public const string DEFAULTVALUE_BOOL = "false";
    public const string DEFAULTVALUE_FLOAT = "0";

    /// <summary>
    /// Action 共有的属性
    /// </summary>
    public static readonly DefalutValue[] ACTION_DEFAULT_VALUE = new DefalutValue[] { };
    /// <summary>
    /// HP action独有的属性
    /// </summary>
    public static readonly DefalutValue[] ACTION_DEFAULT_VALUE_HP = new DefalutValue[] {
        new DefalutValue(PropertiesKey.ACTION_EFFECT_NAME,DEFAULTVALUE_STRING),
        new DefalutValue(PropertiesKey.ACTION_AUDIO_NAME,DEFAULTVALUE_STRING),
        new DefalutValue(PropertiesKey.ACTION_EFFECT_FOLLOW,"false"),
        new DefalutValue(PropertiesKey.ACTION_EFFECT_DURATION,"10"),
    };

    /// <summary>
    /// Motion共有属性
    /// </summary>
    public static readonly DefalutValue[] MOTION_DEFAULT_VALUE = new DefalutValue[] {
        new DefalutValue(PropertiesKey.MOTION_ID, "10000000"),
        new DefalutValue(PropertiesKey.MOTION_OWNER, "10000000"),
        new DefalutValue(PropertiesKey.MOTION_AUTO_DESTROY_TIME, "10"),
        new DefalutValue(PropertiesKey.MOTION_TYPE,DEFAULTVALUE_STRING)
    };
    /// <summary>
    /// 直接触发型的Motion独有属性
    /// </summary>
    public static readonly DefalutValue[] MOTION_DEFAULT_VALUE_DIRECTLYTRIGGER = new DefalutValue[] {
        new DefalutValue(PropertiesKey.MOTION_DELAY_TIME,"0")
    };

    public static readonly DefalutValue[] SUMMON_DEFAULT_VALUR = new DefalutValue[] {
        new DefalutValue(PropertiesKey.SUMMON_ID,"10000000"),
        new DefalutValue(PropertiesKey.SUMMON_OWNER,"10000000"),
        new DefalutValue(PropertiesKey.SUMMON_NAME,DEFAULTVALUE_STRING),
        new DefalutValue(PropertiesKey.SUMMON_TRIGGERCOUNT,"1"),
        new DefalutValue(PropertiesKey.SUMMON_DURATION,"20"),
        //生成效果需要填写
        new DefalutValue(PropertiesKey.SUMMON_CREATE_EFFECT_NAME,DEFAULTVALUE_STRING),
        new DefalutValue(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_TYPE,PropertiesKey.SUMMON_EFFECT_NODE_TYPE_CASTER),
        new DefalutValue(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_NAME,DEFAULTVALUE_STRING),
        new DefalutValue(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_OFFSET,"0,0,0"),
        new DefalutValue(PropertiesKey.SUMMON_CREATE_AUDIO_NAME,DEFAULTVALUE_STRING),
        //触发效果需要填写
        new DefalutValue(PropertiesKey.SUMMON_TRIGGER_EFFECT_NAME,DEFAULTVALUE_STRING),
        new DefalutValue(PropertiesKey.SUMMON_TRIGGER_EFFECT_NODE_OFFSET,"0,0,0"),
        new DefalutValue(PropertiesKey.SUMMON_TRIGGER_AUDIO_NAME,DEFAULTVALUE_STRING),
        //飞行效果需要填写
        new DefalutValue(PropertiesKey.SUMMON_FLY_EFFECT_NAME,DEFAULTVALUE_STRING)
    };
}
public class DefalutValue
{
    public string key;
    public string default_value;

    public DefalutValue(string key,string default_value)
    {
        this.key = key;
        this.default_value = default_value;
    }
}
