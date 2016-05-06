using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngineInternal;
using Code.SkillSystem.Runtime;

public class WindowSkillEditor : EditorWindow {

    private bool m_Init = false;
    private Rect m_Position;
    private Character m_character;
    private EditorMonoUpdate m_Update;

    #region Resources资源
    private Texture m_t_play;
    private Texture m_t_pause;
    private Texture m_t_playforward;
    private Texture m_t_keyFrame;
    #endregion

    public bool isPlay { get;  set; }
    public bool isPause { get;  set; }
    public float time { get;  set; }
    public double oldTime { get;  set; }
    public float deltaTime { get;  set; }

    void OnEnable()
    {
        //EditorApplication.isPlaying = true;
        Prop.openAddProp += CustomMenuItem.OpenProp;
        SkillProp.CreateSummon += CreateSummon;
        SkillProp.RemoveSummon += RemoveSummon;

        SkillProp.right_draw = SkillProp.DrawSkillType.Skill;
        //数据加载
        //GameDB.Instance.Init();
        GameCenter.Instance.DataManager.Init();
        if (m_character == null)
        {
            GameObject obj = new GameObject("skill_Obj");
            m_character = obj.AddComponent<Character>();
            //m_character.Awake();

            m_Update = obj.AddComponent<EditorMonoUpdate>();
            m_Update.updateDele += MonoUpdate;
        }
        Init();
        EditorApplication.update += InvokeUpdate;
    }

    void OnDisable()
    {
        Prop.openAddProp += CustomMenuItem.OpenProp;
        SkillProp.CreateSummon -= CreateSummon;
        SkillProp.RemoveSummon -= RemoveSummon;

        EditorApplication.update -= InvokeUpdate;
        if (m_character!= null && isPause == false && m_character.gameObject.name.Equals("skill_Obj"))
        {
            GameObject.DestroyImmediate(m_character.gameObject);
        }
        //EditorApplication.isPlaying = false;
    }

    public void Init()
    {
        if (position != m_Position)
        {
            SkillEditorConfig.topRect.width = position.width;

            SkillEditorConfig.bottomRect.y = position.height - SkillEditorConfig.bottomRect.height;
            SkillEditorConfig.bottomRect.width = position.width;

            SkillEditorConfig.centerLeftRect.y = 20;
            SkillEditorConfig.centerLeftRect.height = position.height - SkillEditorConfig.topRect.height - SkillEditorConfig.bottomRect.height;

            SkillEditorConfig.centerRightRect.x = SkillEditorConfig.centerLeftRect.width;
            SkillEditorConfig.centerRightRect.y = 20;
            SkillEditorConfig.centerRightRect.width = position.width - SkillEditorConfig.centerLeftRect.width;
            SkillEditorConfig.centerRightRect.height = position.height - SkillEditorConfig.topRect.height - SkillEditorConfig.bottomRect.height;
            m_Position = position;
        }

        if (m_Init) { return; }
        m_Init = true;
        SkillEditorConfig.Init();
        minSize = SkillEditorConfig.minWndSize;
        position = new Rect(position.x, position.y, minSize.x, minSize.y);
        titleContent.text = "技能编辑器";

        SkillEditorConfig.topRect.width = position.width;

        SkillEditorConfig.bottomRect.y = position.height - SkillEditorConfig.bottomRect.height;
        SkillEditorConfig.bottomRect.width = position.width;

        SkillEditorConfig.centerLeftRect.y = 20;
        SkillEditorConfig.centerLeftRect.height = position.height - SkillEditorConfig.topRect.height - SkillEditorConfig.bottomRect.height;

        SkillEditorConfig.centerRightRect.x = SkillEditorConfig.centerLeftRect.width;
        SkillEditorConfig.centerRightRect.y = 20;
        SkillEditorConfig.centerRightRect.width = position.width - SkillEditorConfig.centerLeftRect.width;
        SkillEditorConfig.centerRightRect.height = position.height - SkillEditorConfig.topRect.height - SkillEditorConfig.bottomRect.height;
        m_Position = position;

        #region 加载Resources资源
        m_t_play = Resources.Load("skill_editor_play") as Texture;
        m_t_playforward = Resources.Load("skill_editor_play") as Texture;
        m_t_pause = Resources.Load("skill_editor_play") as Texture;
        m_t_keyFrame = Resources.Load("skill_editor_keyframe") as Texture;
        #endregion

        InitSkill();
    }

    private Dictionary<uint, Skill> m_Skills = new Dictionary<uint, Skill>();
    public void InitSkill()
    {
        m_Skills.Clear();

        foreach(var it in GameCenter.Instance.DataManager.skillDB.data)
        {
            Skill ski = new Skill();
            ski.Create(m_character, it.Value);
            m_Skills.Add(it.Key, ski);
            if(m_SelectSkillIndex == 0)
            {
                m_SelectSkillIndex = it.Key;
            }
        }
    }
    /// <summary>
    /// Update函数
    /// </summary>
    public void InvokeUpdate()
    {
        deltaTime = (float)(EditorApplication.timeSinceStartup - oldTime);
        oldTime = EditorApplication.timeSinceStartup;

        if (isPlay)
        {

            if (m_Skills.ContainsKey(m_SelectSkillIndex))
            {
                m_Skills[m_SelectSkillIndex].Update(deltaTime);
                time += deltaTime;

                if (time > ((float)count / (float)m_CountPreSec))
                {
                    time -= ((float)count / (float)m_CountPreSec);
                }

                Repaint();
            }

        }
    }

    public void MonoUpdate(float elapsed_sec)
    {
        if (isPlay)
        {
            if (m_Skills.ContainsKey(m_SelectSkillIndex))
            {
                m_Skills[m_SelectSkillIndex].UpdateEditor(elapsed_sec);
                time += elapsed_sec;

                if (time > ((float)count / (float)m_CountPreSec))
                {
                    time -= ((float)count / (float)m_CountPreSec);
                }

                Repaint();
            }
        }
    }

    public void OnGUI()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Character>() != null)
        {
            m_character = Selection.activeGameObject.GetComponent<Character>();
            if (m_Skills.ContainsKey(m_SelectSkillIndex))
            {
                m_Skills[m_SelectSkillIndex].ReplaceHoler(m_character);
            }
        }
        Event e = Event.current;

        #region 监听键盘 Shift/ Ctrl 响应时间
        switch (e.modifiers)
        {
            case EventModifiers.None:
                {
                    isControl = false;
                    isShift = false;
                    break;
                }
            case EventModifiers.Shift:
                {
                    isControl = false;
                    isShift = true;
                    break;
                }
            case EventModifiers.Control:
                {
                    isControl = true;
                    isShift = false;
                    break;
                }
        }
        
        //if (e != null)
        //{
        //    if (e.type == EventType.KeyDown)
        //    {
        //        if (e.control)
        //        {
        //            isControl = true;
        //        }
        //        else
        //        {
        //            isControl = false;
        //        }
        //        if (e.alt)
        //        {
        //            isShift = true;
        //        }
        //        else
        //        {
        //            isShift = false;
        //        }
        //    }
        //    else if (e.type == EventType.KeyUp)
        //    {
        //        if (e.control)
        //        {
        //            isControl = true;
        //        }
        //        else
        //        {
        //            isControl = false;
        //        }
        //        if (e.alt)
        //        {
        //            isShift = true;
        //        }
        //        else
        //        {
        //            isShift = false;
        //        }
        //    }
        //}
        #endregion

        #region 监听鼠标右键
        if(m_Select != -1)
        {
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                if (IsRect(SkillEditorConfig.centerRightRect, e.mousePosition))
                {
                    ShowCenterRightContext();
                }
                else if (IsRect(SkillEditorConfig.centerLeftRect, e.mousePosition))
                {
                    ShowCenterLeftContext();
                }
            }
            else
            {

            }
        }
        #endregion

        m_timeLineSize.x = m_OriTimeLineSize.x * m_SliderScale;

        Init();
        DrawTop();
        DrawCenterLeft();
        DrawCenterRight();
        DrawBottom();
    }

    public bool IsRect(Rect rect ,Vector2 click_position)
    {
        return rect.Contains(click_position);
    }

    #region Top Title功能函数
    /// <summary>
    /// Title 内容
    /// </summary>
    public void DrawTop()
    {
        GUI.enabled = true;
        GUI.BeginGroup(SkillEditorConfig.topRect);
        GUILayout.BeginHorizontal("box");
        //GUIStyle styleLabelMenu = new GUIStyle(EditorStyles.toolbarButton);
        //styleLabelMenu.normal.background = null;

        //if (GUITools.DrawButton(SkillEditorConfig.optaionsBtnRect, "Options", EditorStyles.toolbarButton))
        //{
        //    Debug.LogError("Click!!!");
        //}

        //if (GUITools.DrawButton(new Rect(60,0,600,22), "Options", EditorStyles.toolbarButton))
        //{
        //}

        if (isPlay)
        {
            //GUI.color = Color.blue;
            if (GUI.Button(new Rect(0, 0, 22, 22), m_t_play, EditorStyles.toolbarDropDown))
            {
                if (isPlay) { Stop(); }
                else { Play(); }
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 44, 22), m_t_play, EditorStyles.toolbarButton))
            {
                if (isPlay) { Stop(); }
                else { Play(); }
            }
        }

        GUI.color = Color.white;

        //if (isPause) 
        //{
        //    //GUI.color = Color.blue;
        //    if (GUI.Button(new Rect(44, 0, 44, 22), m_t_pause, EditorStyles.toolbarButton))
        //    {
        //        isPause = true;
        //    }
        //}
        //else
        //{
        //    if (GUI.Button(new Rect(44, 0, 44, 22), m_t_pause, EditorStyles.toolbarDropDown))
        //    {
        //        isPause = false;
        //    }
        //}

        //if (GUI.Button(new Rect(88, 0, 44, 22), m_t_playforward, EditorStyles.toolbarButton))
        //{
        //}


        GUI.enabled = !isPlay;

        m_SliderScale = GUI.HorizontalSlider(new Rect(800, 0, 200, 22), m_SliderScale, 0.5f, 5);
        GUI.Label(new Rect(1000, 0, 100, 22), m_SliderScale.ToString());
        GUILayout.EndHorizontal();
        GUI.EndGroup();
    }
    /// <summary>
    /// 技能预览播放
    /// </summary>
    public void Play()
    {
        isPlay = true;
        //EditorApplication.isPlaying = true;
        if (m_Skills.ContainsKey(m_SelectSkillIndex))
        {
            m_Skills[m_SelectSkillIndex].RePlay();
        }
       
    }
    /// <summary>
    /// 技能播放停止
    /// </summary>
    public void Stop()
    {
        isPlay = false;
        //EditorApplication.isPlaying = false;
        ResetPlay();
    }
    public void ResetPlay()
    {
        time = 0;
    }
    /// <summary>
    /// 跳到下一帧
    /// </summary>
    public void NextFrame()
    {
        if (isPlay)
        {
            if (m_Skills.ContainsKey(m_SelectSkillIndex))
            {
                m_Skills[m_SelectSkillIndex].Update(deltaTime);
                time += deltaTime;
            }
        }
    }
    #endregion
    #region Center Right功能函数 技能页面
    public Character character;

    #region SKILL
    private Vector2 m_ScrollTimeLinePos;
    private int count = 1000;
    private Vector2 m_OriTimeLineSize = new Vector2(10, 20);
    private Vector2 m_timeLineSize = new Vector2(10, 20);
    private List<int> m_SelectList = new List<int>();
    private int m_Select = -1;
    private bool isControl = false;
    private bool isShift = false;
    //刻度单元格大小
    private float m_SliderScale = 1;
    public GenericMenu m_EventAdd = new GenericMenu();
    void DrawCenterRight()
    {
        //0->skill 1->summon 2->action
        switch (SkillProp.right_draw)
        {
            case SkillProp.DrawSkillType.Skill:
                {
                    DrawCenterRightSkill();
                    break;
                }
            case SkillProp.DrawSkillType.Summon:
                {
                    DrawSummon();
                    break;
                }
            case SkillProp.DrawSkillType.Action:
                {
                    DrawAction();
                    break;
                }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void DrawCenterRightSkill()
    {
        GUI.Box(SkillEditorConfig.centerRightRect, "", GUI.skin.box);
        GUI.enabled = !isPlay;
        //GUI.BeginGroup(SkillEditorConfig.centerRightRect);
        Rect scrollPos = new Rect(SkillEditorConfig.centerRightRect.x, SkillEditorConfig.centerRightRect.y, SkillEditorConfig.centerRightRect.width, SkillEditorConfig.centerRightRect.height);
        Rect scrollView = new Rect(SkillEditorConfig.centerRightRect.x, SkillEditorConfig.centerRightRect.y, m_timeLineSize.x * count, SkillEditorConfig.centerRightRect.height);

        m_ScrollTimeLinePos = GUI.BeginScrollView(scrollPos, m_ScrollTimeLinePos, scrollView, GUI.skin.horizontalScrollbar, GUIStyle.none);

        #region 刻度数字
        //GUI.BeginGroup(new Rect(0, 0, SkillEditorConfig.centerRightRect.width, m_timeLineSize.y));

        if (m_Skills.ContainsKey(m_SelectSkillIndex))
        {
            count = (int)(m_Skills[m_SelectSkillIndex].skillStaticData.skill_time * m_CountPreSec);
        }

        for (int i = 1; i <= count; i++)
        {
            if (i == 1)
            {
                GUI.Label(new Rect(SkillEditorConfig.centerRightRect.x + (i - 1) * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y, 100, m_timeLineSize.y), GetTime(i));
            }
            else
            {
                if (m_SliderScale >= 0 && m_SliderScale < 0.85f)
                {
                    if (i % 10 == 0)
                    {
                        GUI.Label(new Rect(SkillEditorConfig.centerRightRect.x + (i - 1) * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y, 100, m_timeLineSize.y), GetTime(i));
                    }
                }
                else if (m_SliderScale >= 0.85f && m_SliderScale < 4)
                {
                    if (i % 5 == 0)
                    {
                        GUI.Label(new Rect(SkillEditorConfig.centerRightRect.x + (i - 1) * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y, 100, m_timeLineSize.y), GetTime(i));
                    }
                }
                else
                {
                    GUI.Label(new Rect(SkillEditorConfig.centerRightRect.x + (i - 1) * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y, 100, m_timeLineSize.y), GetTime(i));
                }

                if (i % 5 == 0)
                {
                    GUI.color = Color.gray;
                    if (i % 10 == 0)
                    {
                        GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + (i - 1) * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y, 1, m_timeLineSize.y), EditorGUIUtility.whiteTexture);
                    }
                    else
                    {
                        GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + (i - 1) * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y * 0.5f, 1, m_timeLineSize.y * 0.5f), EditorGUIUtility.whiteTexture);
                    }

                    GUI.color = Color.white;
                    //GUI.Label(new Rect(SkillEditorConfig.centerRightRect.x + (i - 1) * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y, 100, m_timeLineSize.y), GetTime(i));
                }
                else
                {
                    GUI.color = Color.gray;
                    GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + (i - 1) * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y * 0.75f, 1, m_timeLineSize.y * 0.25f), EditorGUIUtility.whiteTexture);
                    GUI.color = Color.white;
                }
            }

        }

        //GUI.EndGroup();
        #endregion

        #region 刻度条按钮
        //GUI.BeginGroup(new Rect(0, m_timeLineSize.y, SkillEditorConfig.centerRightRect.width, m_timeLineSize.y));
        GUILayout.BeginHorizontal();
        for (int i = 0; i < count; i++)
        {
            GUI.color = Color.gray;
            GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + i * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y, 1, m_timeLineSize.y), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + i * m_timeLineSize.x + 4, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y, 1, m_timeLineSize.y), EditorGUIUtility.whiteTexture);

            if (i != 0 && (i - 4) % 5 == 0)
            {
                GUI.color = Color.gray;
            }
            else
            {
                GUI.color = Color.white;
            }

            GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + i * m_timeLineSize.x + 1, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y, m_timeLineSize.x - 2, m_timeLineSize.y), EditorGUIUtility.whiteTexture);

            if (m_SelectList.Contains(i))
            {
                GUI.color = new Color(0, 1, 0, 0.5f);
                GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + i * m_timeLineSize.x + 1, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y, m_timeLineSize.x - 2, m_timeLineSize.y), EditorGUIUtility.whiteTexture);
                GUI.color = Color.white;
            }

            GUI.color = Color.white;
            if (GUI.Button(new Rect(SkillEditorConfig.centerRightRect.x + i * m_timeLineSize.x, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y, m_timeLineSize.x, m_timeLineSize.y), new GUIContent("", GetTime(i + 1)), EditorStyles.whiteLabel))
            {
                if (!isControl && !isShift)
                {
                    m_SelectList.Clear();
                    m_SelectList.Add(i);
                    m_Select = i;
                }
                else
                {
                    if (isControl)
                    {
                        m_SelectList.Add(i);
                    }
                    else
                    {
                        if (isShift)
                        {
                            m_SelectList.Clear();
                            if (m_Select != -1)
                            {
                                if (i >= m_Select)
                                {
                                    for (int j = i; j >= m_Select; j--)
                                    {
                                        m_SelectList.Add(j);
                                    }
                                }
                                else
                                {
                                    for (int j = i; j <= m_Select; j++)
                                    {
                                        m_SelectList.Add(j);
                                    }
                                }
                            }
                            else
                            {
                                m_Select = i;
                            }
                        }
                    }
                }

            }
        }
        GUILayout.EndHorizontal();
        //GUI.EndGroup();
        #endregion

        #region 描绘选中红线
        if (isPlay)
        {
            GUI.color = new Color(0.8f, 0, 0, 1);

            float curFrame = time * m_CountPreSec;

            float page_count = SkillEditorConfig.centerRightRect.width / m_timeLineSize.x;

            int curPage = (int)(curFrame / page_count);

            m_ScrollTimeLinePos = new Vector2(curPage * SkillEditorConfig.centerRightRect.width, m_ScrollTimeLinePos.y);


            GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + (curFrame) * m_timeLineSize.x, m_timeLineSize.y * 1.75f, m_timeLineSize.x, 2), EditorGUIUtility.whiteTexture);

            GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + (curFrame + 0.5f) * m_timeLineSize.x - 1, m_timeLineSize.y * 1.75f, 2, m_timeLineSize.y * 1.75f), EditorGUIUtility.whiteTexture);

            GUI.color = Color.white;
        }
        else
        {
            if (m_Select != -1)
            {
                GUI.color = new Color(0.8f, 0, 0, 1);

                GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + (m_Select) * m_timeLineSize.x, m_timeLineSize.y * 1.75f, m_timeLineSize.x, 2), EditorGUIUtility.whiteTexture);

                GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + (m_Select + 0.5f) * m_timeLineSize.x - 1, m_timeLineSize.y * 1.75f, 2, m_timeLineSize.y * 1.75f), EditorGUIUtility.whiteTexture);

                GUI.color = Color.white;
            }
        }
        #endregion

        #region 描绘有时间的关键帧

        GUI.color = Color.black;
        float circle_scale = 8 * m_SliderScale;
        if (circle_scale >= 16)
        {
            circle_scale = 16;
        }
        if (m_Skills.ContainsKey(m_SelectSkillIndex))
        {
            foreach (var it in m_Skills[m_SelectSkillIndex].timeLine.timeEvents)
            {
                int curFrame = (int)(it.time * m_CountPreSec);
                GUI.DrawTexture(new Rect(SkillEditorConfig.centerRightRect.x + (curFrame + 0.5f) * m_timeLineSize.x - circle_scale * 0.5f, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y * 2 - circle_scale, circle_scale, circle_scale), m_t_keyFrame);

            }
        }
        GUI.color = Color.white;
        #endregion
        GUI.EndScrollView();

        //GUI.EndGroup();
        DrawSkillInformation();
    }
    public string GetTime(int count)
    {
        string str = string.Empty;

        int sec = count / m_CountPreSec;

        //if(sec < 10)
        //{
        //    str = "0";
        //    str += sec.ToString();
        //}
        //else
        //{
        //    str = sec.ToString();
        //}
        str = sec.ToString();
        str += ":";

        int minSec = count % m_CountPreSec;

        if (minSec < 10)
        {
            str += "0";
            str += minSec.ToString();
        }
        else
        {
            str += minSec.ToString();
        }
        return str;
    }
    public void ShowCenterRightContext()
    {
        m_EventAdd = new GenericMenu();
        m_EventAdd.AddItem(new GUIContent("Event/Add Summon Event"), false, InvokeCenterRightContextMenuItem, 0);
        m_EventAdd.AddItem(new GUIContent("Event/Add Action Event"), false, InvokeCenterRightContextMenuItem, 1);
        m_EventAdd.AddSeparator("Event/");
        m_EventAdd.ShowAsContext();
    }
    void InvokeCenterRightContextMenuItem(object _index)
    {
        int index = (int)_index;

        if (m_Skills.ContainsKey(m_SelectSkillIndex))
        {
            //描绘技能信息
            Skill draw_skill = m_Skills[m_SelectSkillIndex];

            switch (index)
            {
                case 0:
                    {
                        for (int i = 0; i < m_SelectList.Count; i++)
                        {
                            TimeEventSkillSummon summon_event = new TimeEventSkillSummon();
                            SummonData summon_data = new SummonData(new Prop());
                            //添加默认属性
                            GameCenter.Instance.DataManager.skillSummonDB.AddDefault(summon_data.prop);
                            //设置飞行物owner
                            summon_data.owner = m_SelectSkillIndex;
                            summon_data.prop.SetValue(PropertiesKey.SUMMON_OWNER, m_SelectSkillIndex.ToString());
                            //设置飞行物id
                            summon_data.id = GameCenter.Instance.DataManager.skillSummonDB.MaxID(summon_data) + 1;
                            summon_data.prop.SetValue(PropertiesKey.SUMMON_ID, summon_data.id.ToString());
                            //添加进入数据库
                            GameCenter.Instance.DataManager.skillSummonDB.Add(summon_data);
                            summon_event.summon_id = (int)summon_data.id;
                            summon_event.time = m_SelectList[i] * m_CellTime + 0.5f * m_CellTime;
                            //summon_event.time = (float)m_SelectList[i] / (float)m_CountPreSec;
                            //添加事件关键帧
                            draw_skill.AddTimeEvent(summon_event);
                        }

                        break;
                    }
                case 1:
                    {
                        break;
                    }
            }
        }


    }
    void DrawSkillInformation()
    {
        if (m_SelectSkillIndex != 0)
        {
            Rect rect = new Rect(SkillEditorConfig.centerRightRect.x, SkillEditorConfig.centerRightRect.y + m_timeLineSize.y * 2.5f, SkillEditorConfig.centerRightRect.width, SkillEditorConfig.centerRightRect.height - m_timeLineSize.y * 2.5f);

            GUILayout.BeginArea(rect, GUI.skin.box);

            GUILayout.BeginVertical();

            //描绘技能的基本信息
            if (m_Skills.ContainsKey(m_SelectSkillIndex))
            {
                //描绘技能信息
                Skill draw_skill = m_Skills[m_SelectSkillIndex];
                draw_skill.Draw(!isPlay);
                GUILayout.BeginVertical("box");
                //描绘时间轴信息
                for (int i = m_Skills[m_SelectSkillIndex].timeLine.timeEvents.Count - 1; i >= 0; i--)
                {
                    int curFrame = (int)(m_Skills[m_SelectSkillIndex].timeLine.timeEvents[i].time * m_CountPreSec);

                    if (curFrame == m_Select)
                    {
                        m_Skills[m_SelectSkillIndex].timeLine.timeEvents[i].Draw();
                    }
                }
                //foreach (var it in m_Skills[m_SelectSkillIndex].timeLine.timeEvents)
                //{
                //    int curFrame = (int)(it.time * m_CountPreSec);

                //    if (curFrame == m_Select)
                //    {
                //        it.Draw();
                //    }
                //}
                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();

            //GUI.Box(rect, "", GUI.skin.box);

            ////描绘技能的基本信息
            //uint select_id = uint.Parse(GameDB.Instance().skillDB.GetSkillIsList[m_SelectSkillIndex]);

            //if (m_Skills.ContainsKey(select_id))
            //{
            //    Skill draw_skill = m_Skills[select_id];

            //}
        }
    }
    #endregion

    #region SUMMON
    Code.SkillSystem.Runtime.Summon summon = null;
    Vector2 m_scrollPos;

    private void DrawSummon()
    {
        GUILayout.BeginArea(SkillEditorConfig.centerRightRect);
        m_scrollPos = GUILayout.BeginScrollView(m_scrollPos);
        if (summon != null)
        {
            summon.Draw();
        }
        if (GUILayout.Button("返回"))
        {
            //Save();
            SkillProp.right_draw = SkillProp.DrawSkillType.Skill;


        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public void CreateSummon(uint owner,uint id)
    {
        summon = new Code.SkillSystem.Runtime.Summon();
        summon.LoadData(m_character, owner,id);
    }

    public void RemoveSummon(uint owner, uint id)
    {
        summon = new Code.SkillSystem.Runtime.Summon();
        summon.Remove(owner, id);
    }

    #endregion

    void DrawAction() { }

    #endregion
    #region Center Left功能函数
    private int m_CountPreSec = 60;
    private float m_CellTime = 0.0001f;
    private uint m_SelectSkillIndex = 0;
    private Vector2 m_ScrollLeft;
    private GenericMenu m_LeftContextMenu = new GenericMenu();
    private int m_WndType = 0;
    
    private string[] m_WndTypeArr = new string[3] { "技能界面", "飞行物界面", "行为界面" };
    public void DrawCenterLeft()
    {
        GUI.enabled = !isPlay;
        GUI.Box(SkillEditorConfig.centerLeftRect, "", GUI.skin.box);
        GUI.BeginGroup(SkillEditorConfig.centerLeftRect);

        //Title
        m_WndType = EditorGUI.Popup(new Rect(0, 0, 100, 16), m_WndType, m_WndTypeArr);
        EditorGUI.LabelField(new Rect(100, 0, 50, 16), "Sample", EditorStyles.toolbarButton);
        m_CountPreSec = EditorGUI.IntField(new Rect(150, 0, 50, 16), m_CountPreSec);
        m_CellTime = 1.0f / (float)m_CountPreSec;
        if (m_CountPreSec <= 0)
        {
            m_CountPreSec = 1;
        }
        Rect scrollPos = new Rect(0, 20, SkillEditorConfig.centerLeftRect.width, SkillEditorConfig.centerLeftRect.height);
        Rect scrollView = new Rect(0, 20, GameCenter.Instance.DataManager.skillDB.data.Count * 20 + 20, SkillEditorConfig.centerLeftRect.height);

        m_ScrollLeft = GUI.BeginScrollView(scrollPos, m_ScrollLeft, scrollView);

        switch (m_WndType)
        {
            case 0:
                {
                    int i = 0;
                    foreach (var it in m_Skills)
                    {
                        if (it.Key == m_SelectSkillIndex)
                        {
                            GUI.color = Color.green;
                        }

                        if (GUI.Button(new Rect(0, i * 20 + 20, 180, 20), new GUIContent(it.Key.ToString(), it.Value.skillStaticData.name), EditorStyles.toolbarButton))
                        {
                            if (Event.current.button == 0)
                            {
                                m_SelectSkillIndex = it.Key;
                                SkillProp.right_draw = SkillProp.DrawSkillType.Skill;
                            }
                        }
                        GUI.color = Color.white;
                        i++;
                    }
                    break;
                }
            case 1:
                {
                    break;
                }
            case 2:
                {
                    break;
                }
        }

        

        GUI.EndScrollView();
        GUI.EndGroup();

    }

    public void ShowCenterLeftContext()
    {
        m_LeftContextMenu = new GenericMenu();
        m_LeftContextMenu.AddItem(new GUIContent("Add New Skill"), false, InvokeCenterLeftContextMenuItem, 0);
        m_LeftContextMenu.ShowAsContext();
    }

    void InvokeCenterLeftContextMenuItem(object _index) 
    {
        int index = (int)_index;
        switch(index)
        {
            case 0:
                {
                    WindowAddNewSkill.OpenWindow(new SkillStaticData());
                    break;
                }
        }
    }
    #endregion
    #region Bottom功能函数
    /// <summary>
    /// 底边
    /// </summary>
    public void DrawBottom()
    {
        GUI.Box(SkillEditorConfig.bottomRect, "", GUI.skin.box);

        GUI.BeginGroup(SkillEditorConfig.bottomRect);

        if (GUI.Button(new Rect(0, 0, 100, 100), new GUIContent("Export",""), EditorStyles.toolbarButton))
        {
            Export();
        }

        GUI.EndGroup();
    }
    /// <summary>
    /// 导出资源
    /// </summary>
    public void Export()
    {
        //保存技能信息
        List<SkillStaticData> skillData = new List<SkillStaticData>();
        foreach (var it in m_Skills)
        {
            skillData.Add(it.Value.skillStaticData);
        }
        LocalDB.instance.CreateTable("skill_common", skillData);

        //保存飞行物信息
        GameCenter.Instance.DataManager.skillSummonDB.Save();

        //保存

        //保存时间轴事件
        List<TimeEvent> time_events = new List<TimeEvent>();
        foreach (var it in m_Skills)
        {
            time_events.AddRange(it.Value.timeLine.timeEvents);
        }
        LocalDB.instance.ExecuteNonQuery("delete from time_events");
        LocalDB.instance.CreateTable("time_events",time_events); 

        //保存Motion
        GameCenter.Instance.DataManager.skillMotionDB.Save();
        
        LocalDB.instance.BackupDatabase();

        CustomMenuItem.BackUpDataBase();
    }

    #endregion
}
