using Code.External.Engine.Sqlite;
using Code.SkillSystem;
using System;

public class ConvertStringToSMTarget : IDataConverter
{
    public bool CanConvert(Type fromType, Type toType)
    {
        if (fromType == typeof(SMTargetGet))
        {
            return true;
        }
        return false;
    }

    public object ConvertToDBValue(System.Type valueType, object value, System.Type dbType)
    {
        if (value == null)
            return null;

        return value.ToString();
    }

    public object ConvertToValue(object dbValue, System.Type valueType)
    {
        SMTargetGet sm_target = Activator.CreateInstance(valueType) as SMTargetGet;

        string str = dbValue.ToStringOrEmpty();
        if (string.IsNullOrEmpty(str))
        {
            return sm_target;
        }
        sm_target.Convert(str);

        return sm_target;
    }
}
