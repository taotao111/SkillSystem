using UnityEngine;
using System.Collections.Generic;

namespace Code.SkillSystem
{
    public class SkillHub
    {
        private Dictionary<uint, Skill> m_Skills = new Dictionary<uint, Skill>();
        public Dictionary<uint, Skill> skill { get { return m_Skills; } }
        public ISkillCaster holder;
        public SkillHub(ISkillCaster caster)
        {
            holder = caster;
        }

        /// <summary>
        /// 添加技能
        /// </summary>
        /// <param name="id"></param>
        public Skill AddSkill(uint id)
        {
            Skill ski = new Skill();
            ski.Create(holder, GameCenter.Instance.DataManager.skillDB.Get(id));
            m_Skills.Add(id, ski);

            return ski;
        }
        public void RemoveSkill(uint id)
        {
            if(m_Skills.ContainsKey(id))
            {
                m_Skills.Remove(id);
            }
        }
        public void RemoveSkill(Skill sk)
        {
            RemoveSkill(sk.skillStaticData.id);
        }
        public Skill GetSkill(uint id)
        {
            if (m_Skills.ContainsKey(id)) { return m_Skills[id]; }

            return null;
        }
        public void ReleaseSkill(uint id, IMessage message)
        {
            if (m_Skills.ContainsKey(id)) { ReleaseSkill(m_Skills[id], message); }
        }

        public void ReleaseSkill(Skill sk, IMessage message)
        {
            sk.HandleMessage(message);
        }

        public void Update(float elapsed_sec)
        {
            foreach(KeyValuePair<uint,Skill> sk in m_Skills)
            {
                sk.Value.Update(elapsed_sec);
            }
        }
    }
}