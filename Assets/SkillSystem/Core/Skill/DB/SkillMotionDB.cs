using System.Collections.Generic;
using Code.External.Engine.Sqlite;
using Mono.Data.Sqlite;
namespace Code.SkillSystem
{
    public class SkillMotionDB : DBReader
    {
        private Dictionary<uint, Dictionary<uint, List<Prop>>> m_Datas = new Dictionary<uint, Dictionary<uint, List<Prop>>>();

        public SkillMotionDB(string file_name)
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
        public List<Motion> GetMotion(uint owner_skill, uint owner, Summon summon)
        {
            List<Motion> motions = new List<Motion>();

            List<Prop> props = new List<Prop>();
            if (!m_Datas.ContainsKey(owner_skill)) { return motions; }
            if (m_Datas[owner_skill].TryGetValue(owner, out props))
            {
                for (int i = 0; i < props.Count; i++)
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly(); // 获取当前程序集 

                    Motion motion = (Motion)assembly.CreateInstance("Code.SkillSystem." + props[i].GetString(PropertiesKey.MOTION_TYPE));
                    
                    motion.Create(props[i], summon);
                    motions.Add(motion);
                }
            }
            return motions;
        }
        public void Add(Prop prop)
        {
            uint owner_skill = prop.GetUint(PropertiesKey.MOTION_OWNER_SKILL);
            uint owner_summon = prop.GetUint(PropertiesKey.MOTION_OWNER);
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
#if UNITY_EDITOR

        public void Remove(Prop prop)
        {
            db.Remove(prop);

            //
            uint owner_skill = prop.GetUint(PropertiesKey.MOTION_OWNER_SKILL);
            uint owner_summon = prop.GetUint(PropertiesKey.MOTION_OWNER);

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

                string_value.Append(db[i].GetString(PropertiesKey.MOTION_ID));
                string_value.Append(",");

                string_value.Append(db[i].GetString(PropertiesKey.MOTION_OWNER));
                string_value.Append(",");

                string_value.Append(db[i].GetString(PropertiesKey.MOTION_OWNER_SKILL));
                string_value.Append(",");

                string_value.Append("'");
                string_value.Append(db[i].ToStringExpect(PropertiesKey.MOTION_ID, PropertiesKey.MOTION_OWNER,PropertiesKey.MOTION_OWNER_SKILL));
                string_value.Append("'");

                LocalDB.instance.ExecuteNonQuery(string.Format(string_format, string_type, string_value.ToString()));
            }
        }

        public int MaxID(Prop prop)
        {
            int id = 10000000;
            uint owner_skill = prop.GetUint(PropertiesKey.MOTION_OWNER_SKILL);
            uint owner_summon = prop.GetUint(PropertiesKey.MOTION_OWNER);
            if (m_Datas.ContainsKey(owner_skill) && m_Datas[owner_skill].ContainsKey(owner_summon))
            {
                foreach (var it in m_Datas[owner_skill][owner_summon])
                {
                    if (id < it.GetInt(PropertiesKey.MOTION_ID, 10000000))
                    {
                        id = it.GetInt(PropertiesKey.ACTION_ID, 10000000);
                    }
                }
            }

            return id;
        }
#endif
    }
}