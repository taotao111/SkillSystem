using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Code.SkillSystem.Runtime
{
    public interface IMotion
    {
        void UpdateFrame(float elapsed_sec);
    }

    public class Motion : ScriptableObject, IMotion, IBuildable<Summon>
    {
        public Summon Summon { get; protected set; }
        protected int id;
        protected int owner;
        protected Prop m_Prop = new Prop();
        protected float m_AutoFreeTime = 10;

        public Prop prop
        {
            get
            {
                return m_Prop;
            }
        }
        /// <summary>
        /// 创建Motion，初始化数据
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="summon"></param>
        public void Create(Prop prop,Summon summon)
        {
            Summon = summon;
            m_Prop = prop;
            
            m_AutoFreeTime = m_Prop.GetFloat(PropertiesKey.MOTION_AUTO_DESTROY_TIME);

            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            _Init();
#if UNITY_EDITOR
            Init_Editor();
#endif
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="elapsed_sec"></param>
        public virtual void UpdateFrame(float elapsed_sec)
        {
            _Update(elapsed_sec);
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void _Init()
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed_sec"></param>
        public virtual void _Update(float elapsed_sec) {}
        /// <summary>
        /// 满条件，响应触发
        /// </summary>
        /// <param name="common_trigger"></param>
        public void Trigger(bool common_trigger)
        {
            Summon.Trigger(common_trigger);
        }

#if UNITY_EDITOR
        public bool draw_gui = true;
        public void Init_Editor()
        {
            DrawStyleInt draw_stype_id = new DrawStyleInt(PropertiesKey.MOTION_ID);
            draw_stype_id.enable = false;
            prop.AddStyle(draw_stype_id);

            DrawStyleInt draw_stype_owner = new DrawStyleInt(PropertiesKey.MOTION_OWNER);
            draw_stype_owner.enable = false;
            prop.AddStyle(draw_stype_owner);

            DrawStyleInt draw_stype_owner_skill = new DrawStyleInt(PropertiesKey.MOTION_OWNER_SKILL);
            draw_stype_owner_skill.enable = false;
            prop.AddStyle(draw_stype_owner_skill);

            DrawStyle draw_stype_motion_type = new DrawStyle(PropertiesKey.MOTION_TYPE);
            draw_stype_motion_type.enable = false;
            prop.AddStyle(draw_stype_motion_type);
        }
        public void Create_Editor(Prop prop, Summon summon) { m_Prop = prop; Summon = summon; }
        public void Draw()
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            draw_gui = EditorGUILayout.Foldout(draw_gui,prop.GetString(PropertiesKey.MOTION_TYPE));

            GUI.color = Color.red;
            if (GUILayout.Button("删除"))
            {
                Summon.RemoveMotion(this);
            }
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            if (draw_gui)
            {
                _Draw();
            }

            GUILayout.EndVertical();
        }
        public virtual void _Draw()
        {
            m_Prop.Draw("属性",false);
        }
        /// <summary>
        /// 添加默认属性
        /// </summary>
        /// <param name="prop"></param>
        public virtual void AddDefault(Prop prop)
        {
            for (int i = 0; i < SkillDefaultValue.MOTION_DEFAULT_VALUE.Length; i++)
            {
                prop.Add(SkillDefaultValue.MOTION_DEFAULT_VALUE[i].key, SkillDefaultValue.MOTION_DEFAULT_VALUE[i].default_value);
            }

            prop.SetValue(PropertiesKey.MOTION_TYPE, this.ToString().Replace("Code.SkillSystem.Runtime.", ""));

            Init_Editor();
        }
        public virtual void Export()
        {
            string string_format = "insert into skill_motion ({0}), values ({1})";

            string string_type = "id,owner,prop";

            System.Text.StringBuilder string_value = new System.Text.StringBuilder();

            string_value.Append(id);
            string_value.Append(",");

            string_value.Append(owner);
            string_value.Append(",");


            string_value.Append(prop.ToString());


            LocalDB.instance.ExecuteNonQuery(string.Format(string_format, string_type, string_value.ToString()));
        }
#endif
    }
}