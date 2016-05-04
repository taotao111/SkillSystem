using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class SkillEditorConfig : MonoBehaviour {
    public static Vector2 minWndSize;
    public static Rect topRect;
    public static Rect centerLeftRect;
    public static Rect centerRightRect;
    public static Rect bottomRect;
    public static Rect optaionsBtnRect;

    static SkillEditorConfig()
    {
        Init();
    }

    public static void Init()
    {
        XmlDocument xml = new XmlDocument();

        //string path = Application.dataPath + "/Editor/Config/SkillEditor/SkilllWnd_Config.xml";

        TextAsset textasset = Resources.Load("SkilllWnd_Config") as TextAsset;
        xml.LoadXml(textasset.text);
        //xml.Load(path);
        if (xml.InnerXml != null)
        {
            XmlNode root = xml.SelectSingleNode("config") as XmlNode;
            Dictionary<string, object> data = Helper.Deserialize(root);


            minWndSize = (Vector2)data["minWndSize"];
            topRect = (Rect)data["topRect"];
            centerLeftRect = (Rect)data["centerLeftRect"];
            centerRightRect = (Rect)data["centerRightRect"];
            bottomRect = (Rect)data["bottomRect"];
            optaionsBtnRect = (Rect)data["optaionsBtnRect"];

        }
    }
}

