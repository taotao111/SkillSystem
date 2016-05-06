using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Code.SkillSystem.Runtime
{
    public class SkillSummonDB : DBReader
    {
        private Dictionary<uint, Dictionary<uint,SummonData>> m_Datas = new Dictionary<uint, Dictionary<uint, SummonData>>();

        public SkillSummonDB(string file_name)
            : base(file_name) 
        {
            
        }

        public override void InitSqlite()
        {
            base.InitSqlite();

            for (int i = 0; i < db.Count; i++)
            {
                Add(new SummonData(db[i]));
            }
        }

        public SummonData Get(uint owner,uint id)
        {
            SummonData summon_data = null;
            if (m_Datas.ContainsKey(owner))
            {
                if (!m_Datas[owner].TryGetValue(id,out summon_data)) { Debug.LogError("Don't have the id !!!"); }
                
            }
            return summon_data;
        }
        public void Add(SummonData summon_data)
        {
            if (!m_Datas.ContainsKey(summon_data.owner))
            {
                m_Datas.Add(summon_data.owner, new Dictionary<uint, SummonData>());
            }
            m_Datas[summon_data.owner].Add(summon_data.id, summon_data);
        }

        #region 编辑器脚本
#if UNITY_EDITOR
        public void AddDefault(Prop prop)
        {
            for (int i = 0; i < SkillDefaultValue.SUMMON_DEFAULT_VALUR.Length; i++)
            {
                prop.Add(SkillDefaultValue.SUMMON_DEFAULT_VALUR[i].key, SkillDefaultValue.SUMMON_DEFAULT_VALUR[i].default_value);

                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 

                DrawStyle drawstyle = assembly.CreateInstance(SkillDefaultValue.SUMMON_DEFAULT_VALUR[i].drawstyle_type.ToString().Replace("DS", "DrawStyle"), true, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { SkillDefaultValue.SUMMON_DEFAULT_VALUR[i].key }, null, null) as DrawStyle;
                drawstyle.SetDefaultValue(SkillDefaultValue.SUMMON_DEFAULT_VALUR[i].drawstyle_external_value);
                drawstyle.enable = SkillDefaultValue.SUMMON_DEFAULT_VALUR[i].drawstyle_enable;

                prop.AddStyle(drawstyle);
            }
        }


        public void Remove(SummonData summon_data)
        {
            m_Datas[summon_data.owner].Remove(summon_data.id);
            db.Remove(summon_data.prop);
        }
        public void Save()
        {
            LocalDB.instance.ExecuteNonQuery("delete from " + m_FileName);
            foreach (var it in m_Datas)
            {
                foreach (var summon in it.Value)
                {
                    string string_format = "insert into " + m_FileName + " ({0})  values ({1})";

                    string string_type = "id,name,owner,prop";

                    System.Text.StringBuilder string_value = new System.Text.StringBuilder();

                    string_value.Append(summon.Value.prop.GetUint(PropertiesKey.SUMMON_ID));
                    string_value.Append(",");

                    string_value.Append("'" + summon.Value.prop.GetString(PropertiesKey.SUMMON_NAME) + "'");
                    string_value.Append(",");

                    string_value.Append(summon.Value.prop.GetUint(PropertiesKey.SUMMON_OWNER));
                    string_value.Append(",");

                    string_value.Append("'");
                    string_value.Append(summon.Value.prop.ToStringExpect(PropertiesKey.SUMMON_ID, PropertiesKey.SUMMON_NAME, PropertiesKey.SUMMON_OWNER));
                    string_value.Append("'");

                    LocalDB.instance.ExecuteNonQuery(string.Format(string_format, string_type, string_value.ToString()));
                }
            }
        }
        public uint MaxID(SummonData summon_data)
        {
            uint id = 10000000;

            if (m_Datas.ContainsKey(summon_data.owner))
            {
                foreach (var it in m_Datas[summon_data.owner])
                {
                    if (id < it.Key)
                    {
                        id = it.Key;
                    }
                }
            }

            return id;
        }
#endif
        #endregion
    }
}