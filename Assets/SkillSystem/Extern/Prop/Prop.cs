using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Prop
{
    Dictionary<string, string> mProperties = new Dictionary<string, string>();

    

    public Prop(string[] p_list, char splitChar = ':')
    {
        foreach (var element in p_list)
        {
            string regular_element = Regulate(element, splitChar);
            if (regular_element.Contains(":"))
            {
                string[] pairs = regular_element.Split(':');
                if (pairs.Length == 2)
                {
                    mProperties.Add(pairs[0].Trim(), pairs[1].Trim());
                }
            }
        }
    }
    public Prop(List<string> p_list, char splitChar = ':')
    {
        foreach (var element in p_list)
        {
            string regular_element = Regulate(element, splitChar);
            if (regular_element.Contains(":"))
            {
                string[] pairs = regular_element.Split(':');
                if (pairs.Length == 2)
                {
                    mProperties.Add(pairs[0].Trim(), pairs[1].Trim());
                }
            }
        }
    }
    public Prop(Prop prop)
    {
        foreach (var it in prop.mProperties)
        {
            mProperties.Add(it.Key, it.Value);
        }
    }
    public Prop()
    {
    }
    public static Prop GetProp(params string[] props)
    {
        return new Prop(props);
    }
    public static Prop GetPropByArray(string[] props)
    {
        return new Prop(props);
    }
    public bool GetBool(string key, bool default_value)
    {
        if (HasValue(key))
        {
            return bool.Parse(mProperties[key]);
        }
        return default_value;
    }
    public uint GetUint(string key)
    {
        return GetUint(key, 0);
    }
    public uint GetUint(string key, uint default_value)
    {
        if (HasValue(key))
        {
            return uint.Parse(mProperties[key]);
        }
        return default_value;
    }
    public List<uint> GetUintList(string key)
    {
        List<uint> list = new List<uint>();

        if (HasValue(key))
        {
            string[] unitList = mProperties[key].Split(',');
            foreach (var it in unitList)
            {
                list.Add(uint.Parse(it));
            }
            //return uint.Parse(mProperties[key]);
        }
        return list;
    }
    public int GetInt(string key, int default_value)
    {
        if (HasValue(key))
        {
            return int.Parse(mProperties[key]);
        }
        return default_value;
    }
    public float GetFloat(string key)
    {
        return GetFloat(key, 0);
    }
    public float GetFloat(string key, float default_value)
    {
        if (HasValue(key))
        {
            return float.Parse(mProperties[key]);
        }
        return default_value;
    }
    public string GetString(string key)
    {
        return GetString(key, null);
    }
    public string[] GetStringArray(string key)
    {
        return GetString(key, null).Split(',');
    }
    public string GetString(string key, string default_value)
    {
        if (HasValue(key))
        {
            return mProperties[key];
        }
        return default_value;
    }
    public Vector2 GetVector2(string key)
    {
        if (HasValue(key))
        {
            string regular_element = Regulate(mProperties[key]);
            if (regular_element.Contains(","))
            {
                string[] pairs = regular_element.Split(',');
                if (pairs.Length == 2 || pairs.Length == 3)
                {
                    return new Vector2(float.Parse(pairs[0]), float.Parse(pairs[1]));
                }
            }
        }
        return Vector2.zero;
    }
    public Vector3 GetVector3(string key)
    {
        if (HasValue(key))
        {
            string regular_element = Regulate(mProperties[key]);
            if (regular_element.Contains(","))
            {
                string[] pairs = regular_element.Split(',');
                if (pairs.Length == 2)
                {
                    return new Vector3(float.Parse(pairs[0]), float.Parse(pairs[1]));
                }
                else if (pairs.Length == 3)
                {
                    return new Vector3(float.Parse(pairs[0]), float.Parse(pairs[1]), float.Parse(pairs[2]));
                }
            }
        }
        return Vector3.zero;
    }
    public Vector3[] GetVector3Array(string key)
    {
        if (HasValue(key))
        {
            string regular_element = Regulate(mProperties[key]);

            string[] regular_element_array = regular_element.Split(';');

            List<Vector3> temp = new List<Vector3>();

            foreach (var it in regular_element_array)
            {
                if (it.Contains(","))
                {
                    string[] pairs = it.Split(',');
                    if (pairs.Length == 2)
                    {
                        temp.Add(new Vector3(float.Parse(pairs[0]), float.Parse(pairs[1])));
                    }
                    else if (pairs.Length == 3)
                    {
                        temp.Add(new Vector3(float.Parse(pairs[0]), float.Parse(pairs[1]), float.Parse(pairs[2])));
                    }
                }
            }

            return temp.ToArray();
        }
        return new Vector3[] { };
    }
    public bool HasValue(string key)
    {
        return mProperties.ContainsKey(key);
    }
    string Regulate(string element, params char[] unusual)
    {
        string reuslt = element;

        foreach (var c in unusual)
        {
            reuslt = reuslt.Replace(c.ToString(), ":");
        }

        return reuslt.Trim();
    }
    public override string ToString()
    {
        string result = "";
        foreach (var prop in mProperties)
        {
            result += prop.Key + ":" + prop.Value + ";";
            //result += "{" + prop.Key + ":" + prop.Value + "}";
            //result += prop.Key + "->" + prop.Value + ";";
        }
        
        return result;
    }

    public void Add(string key, string value)
    {
        if (mProperties.ContainsKey(key))
        {
            Debug.LogError("Have Same Key!!!");
        }
        else
        {
            mProperties.Add(key, value);
        }
    }
    public void Add(string[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            string[] key_value = value[i].Split(':');
            if (key_value.Length == 2)
            {
                Add(key_value[0],key_value[1]);
            }
        }
    }
    public void Remove(string key)
    {
        if (mProperties.ContainsKey(key))
        {
            mProperties.Remove(key);
        }
    }
    public void SetValue(string key, string value)
    {
        if (mProperties.ContainsKey(key))
        {
            mProperties[key] = value;
        }
        else
        {
            mProperties.Add(key, value);
        }
    }
    public static string[] ConvertToStringArray(string str)
    {
        string[] prop_array = str.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        //string[] prop_array = str.Split(new char[] { '{', '}' }, System.StringSplitOptions.RemoveEmptyEntries);

        //for (int prop_array_index = 0; prop_array_index < prop_array.Length; prop_array_index++)
        //{
        //    prop_array[prop_array_index] = prop_array[prop_array_index].Replace("->", ":");
        //}

        return prop_array;
    }
#if UNITY_EDITOR
    public Dictionary<string, DrawStyle> draw_styles = new Dictionary<string, DrawStyle>();
    public delegate void OpenProp(Prop prop);
    public static OpenProp openAddProp;
    private bool m_Draw = true;
    public void AddStyle(DrawStyle draw_style)
    {
        if (!draw_styles.ContainsKey(draw_style.name))
        {
            draw_styles.Add(draw_style.name, draw_style);
        }
    }
    public List<string> KeyList()
    {
        List<string> keys = new List<string>();

        foreach (var it in mProperties)
        {
            keys.Add(it.Key);
        }

        return keys;
    }
    public void Draw(bool flag = true)
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        m_Draw = EditorGUILayout.Foldout(m_Draw, "属性");
        GUI.color = Color.green;
        if (GUILayout.Button("添加新属性", GUILayout.ExpandWidth(false)))
        {
            //EditorWindow.GetWindow<WindowAddProp>();
            if (openAddProp != null)
            {
                openAddProp(this);
            }
        }
        
        GUI.color = Color.white;

        GUILayout.EndHorizontal();

        //GUILayout.Label("");

        if (m_Draw)
        {
            foreach (var it in KeyList())
            {
                if (draw_styles.ContainsKey(it))
                {
                    if (draw_styles[it].view)
                    {
                        bool gui_enable = GUI.enabled;
                        Color gui_color = GUI.color;
                        GUI.enabled = draw_styles[it].enable;
                        GUI.color = draw_styles[it].color;

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        EditorGUILayout.LabelField(it + ":", GUILayout.Width(it.Length * 10));
                        draw_styles[it].Draw(mProperties[it]);
                        //mProperties[it] = EditorGUILayout.TextArea(mProperties[it]);
                        if (flag)
                        {
                            GUI.color = Color.red;
                            if (GUILayout.Button("删除", GUILayout.Width(40)))
                            {
                                Remove(it);
                            }
                            GUI.color = draw_styles[it].color;
                        }

                        GUILayout.EndHorizontal();

                        GUI.enabled = gui_enable;
                        GUI.color = gui_color;
                    }

                }
                else
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.LabelField(it + ":", GUILayout.Width(it.Length * 10));
                    mProperties[it] = EditorGUILayout.TextArea(mProperties[it]);
                    if (flag)
                    {
                        GUI.color = Color.red;
                        if (GUILayout.Button("删除", GUILayout.Width(40)))
                        {
                            Remove(it);
                        }
                        GUI.color = Color.white;
                    }

                    GUILayout.EndHorizontal();
                }

            }

        }

        

        GUILayout.EndVertical();
    }
    public string ToStringExpect(params string[] expect_keys)
    {
        string result = "";
        foreach (var prop in mProperties)
        {
            bool get_value = true;
            for (int i = 0; i < expect_keys.Length; i ++)
            {
                if (prop.Key.Equals(expect_keys[i]))
                {
                    get_value = false;
                    break;
                }
            }
            if (get_value)
            {
                result += prop.Key + ":" + prop.Value + ";";
            }
        }

        return result;
    }
#endif
}
