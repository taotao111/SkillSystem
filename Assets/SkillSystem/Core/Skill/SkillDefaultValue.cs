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
    public static readonly DefaultValue[] ACTION_DEFAULT_VALUE = new DefaultValue[] { };
    /// <summary>
    /// HP action独有的属性
    /// </summary>
    public static readonly DefaultValue[] ACTION_DEFAULT_VALUE_HP = new DefaultValue[] {

    };
    public static readonly DefaultValue[] ACTION_DEFAULT_VALUE_HIT = new DefaultValue[] {
    };
    public static readonly DefaultValue[] ACTION_DEFAULT_VALUE_PARTICLE = new DefaultValue[] {
        new DefaultValue(PropertiesKey.ACTION_EFFECT_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS),
        new DefaultValue(PropertiesKey.ACTION_EFFECT_FOLLOW,"false",DrawStyleType.DSBool),
        new DefaultValue(PropertiesKey.ACTION_EFFECT_DURATION,"10",DrawStyleType.DSFloat),
    };
    public static readonly DefaultValue[] ACTION_DEFAULT_VALUE_SHAKECAMERA = new DefaultValue[] {
    };
    public static readonly DefaultValue[] ACTION_DEFAULT_VALUE_SUMMON = new DefaultValue[] {
    };
    public static readonly DefaultValue[] ACTION_DEFAULT_VALUE_ADDBUFF = new DefaultValue[] {
    };
    public static readonly DefaultValue[] ACTION_DEFAULT_VALUE_AUDIO = new DefaultValue[] {
        new DefaultValue(PropertiesKey.ACTION_AUDIO_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS),
    };
    /// <summary>
    /// Motion共有属性
    /// </summary>
    public static readonly DefaultValue[] MOTION_DEFAULT_VALUE = new DefaultValue[] {
        new DefaultValue(PropertiesKey.MOTION_ID, "10000000",DrawStyleType.DSInt),
        new DefaultValue(PropertiesKey.MOTION_OWNER, "10000000",DrawStyleType.DSInt),
        new DefaultValue(PropertiesKey.MOTION_AUTO_DESTROY_TIME, "10",DrawStyleType.DSFloat),
        new DefaultValue(PropertiesKey.MOTION_TYPE,DEFAULTVALUE_STRING,DrawStyleType.DS)
    };
    /// <summary>
    /// 直接触发型的Motion独有属性
    /// </summary>
    public static readonly DefaultValue[] MOTION_DEFAULT_VALUE_DIRECTLYTRIGGER = new DefaultValue[] {
        new DefaultValue(PropertiesKey.MOTION_DELAY_TIME,"0",DrawStyleType.DSFloat)
    };

    public static readonly DefaultValue[] SUMMON_DEFAULT_VALUR = new DefaultValue[] {
        new DefaultValue(PropertiesKey.SUMMON_ID,"10000000",DrawStyleType.DSInt) { drawstyle_enable = false },
        new DefaultValue(PropertiesKey.SUMMON_OWNER,"10000000",DrawStyleType.DSInt){ drawstyle_enable = false },
        new DefaultValue(PropertiesKey.SUMMON_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS),
        new DefaultValue(PropertiesKey.SUMMON_TRIGGERCOUNT,"1",DrawStyleType.DSInt),
        new DefaultValue(PropertiesKey.SUMMON_DURATION,"20",DrawStyleType.DSFloat),
        //生成效果需要填写
        new DefaultValue(PropertiesKey.SUMMON_CREATE_EFFECT_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS),
        new DefaultValue(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_TYPE,PropertiesKey.SUMMON_EFFECT_NODE_TYPE_CASTER,DrawStyleType.DSPopup) { drawstyle_external_value = new string[] {PropertiesKey.SUMMON_EFFECT_NODE_TYPE_CASTER, PropertiesKey.SUMMON_EFFECT_NODE_TYPE_SUMMON, PropertiesKey.SUMMON_EFFECT_NODE_TYPE_SCENE }},
        new DefaultValue(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS),
        new DefaultValue(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_OFFSET,"0,0,0",DrawStyleType.DSVector3),
        new DefaultValue(PropertiesKey.SUMMON_CREATE_AUDIO_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS),
        //触发效果需要填写
        new DefaultValue(PropertiesKey.SUMMON_TRIGGER_EFFECT_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS),
        new DefaultValue(PropertiesKey.SUMMON_TRIGGER_EFFECT_NODE_OFFSET,"0,0,0",DrawStyleType.DSVector3),
        new DefaultValue(PropertiesKey.SUMMON_TRIGGER_AUDIO_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS),
        //飞行效果需要填写
        new DefaultValue(PropertiesKey.SUMMON_FLY_EFFECT_NAME,DEFAULTVALUE_STRING,DrawStyleType.DS)
    };
}
public class DefaultValue
{
    public string key;
    public string default_value;
    public DrawStyleType drawstyle_type = DrawStyleType.DS;
    public object drawstyle_external_value = null;
    public bool drawstyle_enable = true;

    public DefaultValue(string key,string default_value, DrawStyleType drawstyle_type)
    {
        this.key = key;
        this.default_value = default_value;
        this.drawstyle_type = drawstyle_type;
    }
}
public enum DrawStyleType
{
    DS,
    DSInt,
    DSFloat,
    DSPopup,
    DSVector3,
    DSBool,
}
