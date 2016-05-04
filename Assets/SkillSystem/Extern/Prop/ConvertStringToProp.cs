using Code.External.Engine.Sqlite;
using System;

public class ConvertStringToProp : IDataConverter
{
    public bool CanConvert(Type fromType, Type toType)
    {
        if (fromType == typeof(Prop))
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
        Prop prop = new Prop();
        string str = dbValue.ToStringOrEmpty();
        if (string.IsNullOrEmpty(str))
        {
            return prop;
        }

        prop.Add(Prop.ConvertToStringArray(str));

        return prop;
    }
}
