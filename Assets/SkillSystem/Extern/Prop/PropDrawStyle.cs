#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
public class DrawStyle
{
    public string name;
    public Color color = Color.white;
    public bool view = true;
    public bool enable = true;

    public DrawStyle(string name)
    {
        this.name = name;
    }
    public virtual void SetDefaultValue(object defalut_value) { }
    public virtual string Draw(string str)
    {
        str = EditorGUILayout.TextArea(str);
        return str;
    }
    public static DrawStyle DefaultStyle(string name)
    {
        return new DrawStyle(name);
    }
    public static DrawStyleInt IntStyle(string name)
    {
        return new DrawStyleInt(name);
    }
    public static DrawStyleFloat FloatStyle(string name)
    {
        return new DrawStyleFloat(name);
    }
    public static DrawStylePopup Popup(string name, string[] popups)
    {
        DrawStylePopup popup = new DrawStylePopup(name);
        popup.enums = popups;
        return popup;
    }
    public static DrawStyleVector3 Vector3Style(string name)
    {
        return new DrawStyleVector3(name);
    }
}
public class DrawStyleInt : DrawStyle
{
    public DrawStyleInt(string name) : base(name)
    { }

    public override string Draw(string str)
    {
        str = EditorGUILayout.IntField(int.Parse(str)).ToString();

        return str;
    }
}
public class DrawStyleFloat : DrawStyle
{
    public DrawStyleFloat(string name) : base(name)
    { }

    public override string Draw(string str)
    {
        str = EditorGUILayout.FloatField(float.Parse(str)).ToString();

        return str;
    }
}
public class DrawStylePopup : DrawStyle
{
    public string[] enums;
    private int index = -1;
    public DrawStylePopup(string name) : base(name)
    {

    }

    public override string Draw(string str)
    {
        if (enums != null)
        {
            if (index == -1)
            {
                for (int i = 0; i < enums.Length; i++)
                {
                    if (enums[i].Equals(str))
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
            {
                index = 0;
                str = enums[0];
            }
            index = EditorGUILayout.Popup(index, enums);
            str = enums[index];
        }


        return str;
    }
    public override void SetDefaultValue(object defalut_value)
    {
        enums = (string[])defalut_value;
    }
}
public class DrawStyleVector3 : DrawStyle
{
    public DrawStyleVector3(string name):base(name)
    { }

    public override string Draw(string str)
    {
        Vector3 vec3 = Vector3.zero;
        if (str.Contains(","))
        {
            string[] pairs = str.Split(',');
            if (pairs.Length == 2)
            {
                vec3.x = float.Parse(pairs[0]);
                vec3.y = float.Parse(pairs[1]);
            }
            else if (pairs.Length == 3)
            {
                vec3.x = float.Parse(pairs[0]);
                vec3.y = float.Parse(pairs[1]);
                vec3.z = float.Parse(pairs[2]);
            }
        }
        
        vec3 = EditorGUILayout.Vector3Field("", vec3);

        return (vec3.x + "," + vec3.y + "," + vec3.z);
    }
}
public class DrawStyleBool : DrawStyle
{
    public DrawStyleBool(string name) : base(name)
    { }
    public override string Draw(string str)
    {
        bool bool_value = bool.Parse(str);

        bool_value = EditorGUILayout.Toggle(bool_value);

        return bool_value.ToString();
    }
}
#endif