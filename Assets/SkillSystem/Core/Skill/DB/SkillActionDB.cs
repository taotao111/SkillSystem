using UnityEngine;
using System.Collections.Generic;
namespace Code.SkillSystem.Runtime
{
    public class SkillActionDB : DBReader
    {
        private Dictionary<uint, Dictionary<uint, List<Prop>>> m_Datas = new Dictionary<uint, Dictionary<uint, List<Prop>>>();
        public SkillActionDB(string file_name)
            : base(file_name) 
        {
            
        }

        public override void InitSqlite()
        {
            base.InitSqlite();
            for (int i = 0; i < db.Count; i++)
            {
                //
                Add(db[i]);
            }
        }
        public void Add(Prop prop)
        {
            uint owner_skill = prop.GetUint(PropertiesKey.ACTION_OWNER_SKILL);
            uint owner_summon = prop.GetUint(PropertiesKey.ACTION_OWNER);
            if (!m_Datas.ContainsKey(owner_skill))
            {
                m_Datas.Add(owner_skill, new Dictionary<uint, List<Prop>>());
            }

            if (!m_Datas[owner_skill].ContainsKey(owner_summon))
            {
                m_Datas[owner_skill].Add(owner_summon, new List<Prop>());
            }

            m_Datas[owner_skill][owner_summon].Add(prop);

            if (!db.Contains(prop))
            {
                db.Add(prop);
            }
        }


        public List<Action> GetAction(uint owner_skill, uint owner_summon, Summon summon)
        {
            List<Action> actions = new List<Action>();

            List<Prop> props = new List<Prop>();
            if (!m_Datas.ContainsKey(owner_skill)) { return actions; }
            if (m_Datas[owner_skill].TryGetValue(owner_summon, out props))
            {
                for (int i = 0; i < props.Count; i++)
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly(); // 获取当前程序集 

                    Action action = (Action)assembly.CreateInstance("Code.SkillSystem.Runtime." + props[i].GetString(PropertiesKey.ACTION_TYPE));

                    action.Create(props[i], summon);
                    actions.Add(action);
                }
            }
            return actions;
        }

#if UNITY_EDITOR
        public int MaxID(Prop prop)
        {
            int id = 10000000;

            uint owner_skill = prop.GetUint(PropertiesKey.ACTION_OWNER_SKILL);
            uint owner_summon = prop.GetUint(PropertiesKey.ACTION_OWNER);
            if (m_Datas.ContainsKey(owner_skill) && m_Datas[owner_skill].ContainsKey(owner_summon))
            {
                foreach (var it in m_Datas[owner_skill][owner_summon])
                {
                    if (id < it.GetInt(PropertiesKey.ACTION_ID, 10000000))
                    {
                        id = it.GetInt(PropertiesKey.ACTION_ID, 10000000);
                    }
                }
            }

            return id;
        }
        public void Remove(uint owner_skill, uint owner_summon)
        {
            List<Action> actions = new List<Action>();

            List<Prop> props = new List<Prop>();
            if (!m_Datas.ContainsKey(owner_skill)) { return ; }
            if (m_Datas[owner_skill].TryGetValue(owner_summon, out props))
            {
                for (int i = 0; i < props.Count; i++) { Remove(props[i]); }
            }
        }
        public void Remove(Prop prop)
        {
            db.Remove(prop);

            //
            uint owner_skill = prop.GetUint(PropertiesKey.ACTION_OWNER_SKILL);
            uint owner_summon = prop.GetUint(PropertiesKey.ACTION_OWNER);

            if (m_Datas.ContainsKey(owner_skill) && m_Datas[owner_skill].ContainsKey(owner_summon))
            {
                m_Datas[owner_skill][owner_summon].Remove(prop);
            }
        }

        public void Save()
        {
            LocalDB.instance.ExecuteNonQuery("delete from " + m_FileName);
            for (int i = 0; i < db.Count; i++)
            {
                string string_format = "insert into " + m_FileName + " ({0})  values ({1})";

                string string_type = "id,owner,owner_skill,prop";

                System.Text.StringBuilder string_value = new System.Text.StringBuilder();

                string_value.Append(db[i].GetString(PropertiesKey.ACTION_ID));
                string_value.Append(",");

                string_value.Append(db[i].GetString(PropertiesKey.ACTION_OWNER));
                string_value.Append(",");

                string_value.Append(db[i].GetString(PropertiesKey.ACTION_OWNER_SKILL));
                string_value.Append(",");

                string_value.Append("'");
                string_value.Append(db[i].ToStringExpect(PropertiesKey.ACTION_ID, PropertiesKey.ACTION_OWNER,PropertiesKey.ACTION_OWNER_SKILL));
                string_value.Append("'");

                LocalDB.instance.ExecuteNonQuery(string.Format(string_format, string_type, string_value.ToString()));
            }
        }
#endif
    }
}