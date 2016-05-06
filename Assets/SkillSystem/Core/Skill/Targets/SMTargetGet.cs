using UnityEngine;

public enum eSMTarget
{
    SMTargetGetNear = 1,
}
namespace Code.SkillSystem.Runtime
{

    public class SMTargetGet
    {
        protected ISkillCaster m_Holder;
        [DataMember(Name = PropertiesKey.SUMMONTARGET_PROP, ConverterType = typeof(ConvertStringToProp))]
        public Prop prop = new Prop();

        public SMTargetGet(ISkillCaster holder)
        {
            m_Holder = holder;
        }

        public virtual void Init() { }

        public virtual System.Collections.Generic.List<ISkillTarget> Get()
        {
            return null;
        }
        public override string ToString()
        {
            return prop.ToString();
        }

        public void Convert(string str)
        {
            prop.Add(Prop.ConvertToStringArray(str));
        }

#if UNITY_EDITOR
        public void Draw()
        {
            GUILayout.BeginVertical("box");
            _Draw();
            prop.Draw();
            GUILayout.EndVertical();
        }

        public virtual void AddDefault(Prop prop)
        {

        }

        protected virtual void _Draw()
        {

        }
#endif
    }
}