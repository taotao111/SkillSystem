using UnityEngine;
using System.Collections.Generic;
using System.Xml;
#if UNITY_EDITOR
using UnityEditor;
#endif
public static class Helper  {
    /// <summary>
    /// 设置父节点
    /// </summary>
    /// <param name="child">子节点</param>
    /// <param name="parent">父节点</param>
    public static void SetParent(Transform child,Transform parent,bool reset = true)
    {
        child.parent = parent;
        if(reset)
        {
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
        }
    }

    public static Dictionary<string,Transform> GetChildrenNodes(this GameObject go)
    {
        Dictionary<string, Transform> nodes = new Dictionary<string, Transform>();
        
        foreach(var it in go.GetComponentsInChildren<Transform>())
        {
            if(nodes.ContainsKey(it.name))
            {
                Debug.LogError("Has same node:" + it.name);
            }
            else
            {
                nodes.Add(it.name, it);
            }
        }

        return nodes;
    }
    public static Dictionary<string, Transform> GetChildrenNodes(this Transform go)
    {
        Dictionary<string, Transform> nodes = new Dictionary<string, Transform>();

        foreach (var it in go.GetComponentsInChildren<Transform>())
        {
            if (nodes.ContainsKey(it.name))
            {
                Debug.LogError("Has same node:" + it.name);
            }
            else
            {
                nodes.Add(it.name, it);
            }
        }

        return nodes;
    }
    public static string PathToBundle(string path)
    {
        path = path.Replace("//", "_");
        path = path.Replace('/', '_');

        path = path.Substring(0, path.LastIndexOf("."));
        path += ".unity3d";
        return path;
    }

    #region XML HELPER
    public static Dictionary<string, object> Deserialize(XmlNode node)
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        foreach (XmlElement it in node.ChildNodes)
        {
            switch (it.Name)
            {
                case "Rect":
                    {
                        if (!param.ContainsKey(it.GetAttribute("Name")))
                        {
                            param.Add(it.GetAttribute("Name"), StringToRect(it.GetAttribute("Value")));
                        }
                        break;
                    }
                case "Vector2":
                    {
                        if (!param.ContainsKey(it.GetAttribute("Name")))
                        {
                            param.Add(it.GetAttribute("Name"), StringToVector2(it.GetAttribute("Value")));
                        }
                        break;
                    }
                case "Vector3":
                    {
                        if (!param.ContainsKey(it.GetAttribute("Name")))
                        {
                            param.Add(it.GetAttribute("Name"), StringToVector3(it.GetAttribute("Value")));
                        }
                        break;
                    }
                case "Float":
                    {
                        if (!param.ContainsKey(it.GetAttribute("Name")))
                        {
                            param.Add(it.GetAttribute("Name"), StringToFloat(it.GetAttribute("Value")));
                        }
                        break;
                    }
                case "Int":
                    {
                        if (!param.ContainsKey(it.GetAttribute("Name")))
                        {
                            param.Add(it.GetAttribute("Name"), StringToInt(it.GetAttribute("Value")));
                        }
                        break;
                    }
                case "String":
                    {
                        if (!param.ContainsKey(it.GetAttribute("Name")))
                        {
                            param.Add(it.GetAttribute("Name"), it.GetAttribute("Value"));
                        }
                        break;
                    }
            }
        }

        return param;
    }
    public static Vector2 StringToVector2(string str)
    {
        string[] val = str.Split(',');
        if (val.Length == 2)
        {
            return new Vector2(float.Parse(val[0]), float.Parse(val[1]));
        }
        else
        {
            Debug.LogError("错误字符串！！！");
            return Vector2.zero;
        }
    }
    public static Vector3 StringToVector3(string str)
    {
        string[] val = str.Split(',');
        if (val.Length == 3)
        {
            return new Vector3(float.Parse(val[0]), float.Parse(val[1]), float.Parse(val[2]));
        }
        else
        {
            Debug.LogError("错误字符串！！！");
            return Vector3.zero;
        }
    }
    public static Rect StringToRect(string str)
    {
        string[] val = str.Split(',');
        if (val.Length == 4)
        {
            return new Rect(float.Parse(val[0]), float.Parse(val[1]), float.Parse(val[2]), float.Parse(val[3]));
        }
        else
        {
            Debug.LogError("错误字符串！！！");
            return new Rect(0, 0, 100, 100);
        }
    }
    public static float StringToFloat(string str)
    {
        return float.Parse(str);
    }
    public static int StringToInt(string str)
    {
        return int.Parse(str);
    }
    #endregion

#if UNITY_EDITOR

#endif
}
