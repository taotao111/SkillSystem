using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Code.External.Engine.Sqlite;
using Mono.Data.Sqlite;
namespace Code.SkillSystem
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
            using (SqliteDataReader reader = LocalDB.instance.ExecuteReader("select * from " + m_FileName))
            {
                while (true)
                {
                    if (!reader.Read())
                        break;

                    uint id = 0;
                    Prop prop = new Prop();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetName(i).Equals(PropertiesKey.SUMMON_PROP))
                        {
                            prop.Add(reader.GetValue(i).ToString().Split(';'));
                        }
                        else
                        {
                            prop.Add(reader.GetName(i), reader.GetValue(i).ToString());
                        }
                    }

                    uint owner = prop.GetUint(PropertiesKey.SUMMON_OWNER);

                    if (!m_Datas.ContainsKey(owner))
                    {
                        m_Datas.Add(owner, new Dictionary<uint, SummonData>());
                    }
                    m_Datas[owner].Add(prop.GetUint(PropertiesKey.SUMMON_ID),new SummonData(prop));
                }
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

        #region 编辑器脚本
#if UNITY_EDITOR
        public void AddDefault(Prop prop)
        {
            for (int i = 0; i < SkillDefaultValue.SUMMON_DEFAULT_VALUR.Length; i++)
            {
                prop.Add(SkillDefaultValue.SUMMON_DEFAULT_VALUR[i].key, SkillDefaultValue.SUMMON_DEFAULT_VALUR[i].default_value);
            }

            DrawStyleInt summon_draw_id = DrawStyle.IntStyle(PropertiesKey.SUMMON_ID);
            summon_draw_id.enable = false;
            prop.AddStyle(summon_draw_id);

            DrawStyleInt ds = DrawStyle.IntStyle(PropertiesKey.SUMMON_OWNER);
            ds.enable = false;
            prop.AddStyle(ds);

            prop.AddStyle(DrawStyle.Popup(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_TYPE,new string[3] {PropertiesKey.SUMMON_EFFECT_NODE_TYPE_CASTER, PropertiesKey.SUMMON_EFFECT_NODE_TYPE_SUMMON, PropertiesKey.SUMMON_EFFECT_NODE_TYPE_SCENE }));

            prop.AddStyle(DrawStyle.Vector3Style(PropertiesKey.SUMMON_CREATE_EFFECT_NODE_OFFSET));
        }

        public void Add(SummonData summon_data)
        {
            if (!m_Datas.ContainsKey(summon_data.owner))
            {
                m_Datas.Add(summon_data.owner, new Dictionary<uint, SummonData>());
            }
            m_Datas[summon_data.owner].Add(summon_data.id, summon_data);
        }
        public void Remove(SummonData summon_data)
        {
            m_Datas[summon_data.owner].Remove(summon_data.id);
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