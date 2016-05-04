using System.Collections.Generic;
namespace Code.SkillSystem
{
    public class SMTargetGetNear : SMTargetGet
    {
        public SMTargetGetNear(ISkillCaster holder)
            : base(holder)
        {
            
        }

        public override List<ISkillTarget> Get()
        {
            List<ISkillTarget> get_value = new List<ISkillTarget>();

            Character near = null;
            float length = 10000000;

            List<Character> characters = GameCenter.Instance.SceneManager.GetCharactersList(CharacterLayer.GetTargetLayer(m_Holder.Layer, 2));
            for (int i = 0; i < characters.Count; i++)
            {
                if ((characters[i].transform.position - m_Holder.Transform.position).magnitude < length)
                {
                    near = characters[i];
                }
            }
            if (near != null)
            {
                get_value.Add(near);
            }

            return get_value;
        }
#if UNITY_EDITOR
        public override void AddDefault(Prop prop)
        {
            base.AddDefault(prop);
        }
#endif
    }
}