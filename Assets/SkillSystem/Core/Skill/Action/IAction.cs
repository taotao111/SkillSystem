using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Code.SkillSystem.Runtime
{
    public interface IAction
    {
        
    }
    public class Action : IAction,IBuildable<Summon>
    {
        protected Summon m_Summon;
        protected Prop m_Prop = new Prop();
        public Prop prop
        {
            get
            {
                return m_Prop;
            }
        }
        public void Create(Prop prop,Summon summon)
        {
            m_Summon = summon;
            m_Prop = prop;
            Init();
#if UNITY_EDITOR
            Init_Editor();
#endif
        }
        public virtual void Init() { }
        public virtual void Update(float elapsed_sec) { }
        public virtual void Do() { }

        #region 编辑器脚本
#if UNITY_EDITOR
        public bool draw_gui = true;
        public void Draw()
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            draw_gui = EditorGUILayout.Foldout(draw_gui, this.ToString());

            GUI.color = Color.red;
            if (GUILayout.Button("删除"))
            {
                m_Summon.RemoveAction(this);
            }
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            if (draw_gui)
            {
                _Draw();
            }

            GUILayout.EndVertical();
        }
        public virtual void _Draw() { m_Prop.Draw("属性",false); }
        public virtual void AddDefault(Prop prop)
        {
            prop.Add(PropertiesKey.ACTION_TYPE, this.ToString().Replace("Code.SkillSystem.Runtime.", ""));
        }
        public void Init_Editor()
        {
            DrawStyleInt draw_stype_id = new DrawStyleInt(PropertiesKey.ACTION_ID);
            draw_stype_id.enable = false;
            prop.AddStyle(draw_stype_id);

            DrawStyleInt draw_stype_owner = new DrawStyleInt(PropertiesKey.ACTION_OWNER);
            draw_stype_owner.enable = false;
            prop.AddStyle(draw_stype_owner);

            DrawStyle draw_stype_action_type = new DrawStyle(PropertiesKey.ACTION_TYPE);
            draw_stype_action_type.enable = false;
            prop.AddStyle(draw_stype_action_type);
        }
#endif
            #endregion

        }
}