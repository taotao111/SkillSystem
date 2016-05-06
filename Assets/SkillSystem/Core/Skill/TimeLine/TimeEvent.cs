using UnityEngine;
using Mono.Data.Sqlite;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Code.SkillSystem.Runtime
{
    public interface ITimeEvent
    {
        void Update(float elapsed_sec);
    }

    public enum TimeEventType
    {
        TimeEventSkillSummon = 1,
    }

    public class TimeEvent : DBDataBase, IBuildable<TimeLine>, ITimeEvent
    {
        [TimeEventAttributes(Name = PropertiesKey.TIMELINE_ID, dbType = "INT")]
        public int id;
        [TimeEventAttributes(Name = PropertiesKey.TIMELINE_OWNER, dbType = "INT")]
        public uint owner;
        [TimeEventAttributes(Name = PropertiesKey.TIMELINE_TIME, dbType = "FLOAT")]
        public float time;
        [TimeEventAttributes(Name = PropertiesKey.TIMELINE_PROP, dbType = "VARCHAR(1000)", ConverterType = typeof(ConvertStringToProp))]
        public Prop prop = new Prop();
        [TimeEventAttributes(Name = PropertiesKey.TIMELINE_EVENT_TYPE, dbType = "INT")]
        public TimeEventType eventType;
        public TimeLine holder;

        public float curTime;

        public TimeEvent()
        {
            //this.owner = holder.id;

            //this.id = id;
        }
        public virtual void Init() { }

        public void Create(Prop prop, TimeLine param)
        {
            this.prop = prop;
            this.holder = param;
            //this.id.GetType().GetCustomAttributes(typeof(TimeEventAttributes), true);
            this.id = prop.GetInt(PropertiesKey.TIMELINE_ID, 0);
        }
        public bool IsExpire { get; protected set; }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            curTime = time;
            IsExpire = false;
        }

        public void Update(float elapsed_sec)
        {
            curTime -= elapsed_sec;
            if (curTime <= 0)
            {
                Trigger();
                IsExpire = true;
            }
        }
        /// <summary>
        /// 触发
        /// </summary>
        public virtual void Trigger()
        {

        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="reader"></param>
        public virtual void Convert(SqliteDataReader reader)
        {
            LocalDB.instance.Fill(reader, this);
        }
#if UNITY_EDITOR
        public bool m_IsDraw = true;
        GUIStyle m_Boxstype = new GUIStyle();
        public virtual void Draw()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            //基本数据
            m_IsDraw = EditorGUILayout.Foldout(m_IsDraw, "时间关键帧 : " + this.ToString());
            GUI.color = Color.red;
            if (GUILayout.Button("Delete", GUILayout.Width(80)))
            {
                this.Remove();
            }
            GUI.color = Color.white;
            GUILayout.EndHorizontal();

            if (m_IsDraw)
            {
                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.LabelField("id:", GUILayout.Width(200));
                id = EditorGUILayout.IntField(id);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.LabelField("time:", GUILayout.Width(200));
                time = EditorGUILayout.FloatField(time);
                GUILayout.EndHorizontal();

                if (prop != null)
                {
                    prop.Draw();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
        public virtual void Remove()
        {
            holder.timeEvents.Remove(this);
        }
#endif
    }
}