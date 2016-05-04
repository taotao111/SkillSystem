using UnityEngine;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using Code.External.Engine.Sqlite;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.SkillSystem
{
    public class SkillDB : DBReader
    {
        private Dictionary<uint, SkillStaticData> m_Datas = new Dictionary<uint, SkillStaticData>();

        public SkillDB(string file_name) : base(file_name) 
        {
            
        }

        public override void InitSqlite()
        {
            //SkillStaticData[] data = LocalDB.instance.ReadTable<SkillStaticData>("skill");

            //for (int i = 0; i < data.Length; i++ )
            //{
            //    if(!m_Datas.ContainsKey(data[i].id))
            //    {
            //        m_Datas.Add(data[i].id,data[i]);
            //    }
            //    else
            //    {
            //        MyLog.LogError("技能ID相同：" + data[i].id);
            //    }
            //}


            using (SqliteDataReader reader = LocalDB.instance.ExecuteReader("select * from " + m_FileName))
            {
                while (true)
                {
                    if (!reader.Read())
                        break;
                    List<string> prop_list = new List<string>();
                    uint id = 0;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetName(i).Equals(PropertiesKey.SKILL_PROP))
                        {
                            string str = reader.GetValue(i).ToString();


                            string[] prop_array = str.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
                            //string[] prop_array = str.Split(new char[] { '{', '}' }, System.StringSplitOptions.RemoveEmptyEntries);

                            //for (int prop_array_index = 0; prop_array_index < prop_array.Length; prop_array_index++ )
                            //{
                            //    prop_array[prop_array_index] = prop_array[prop_array_index].Replace("->",":");
                            //    prop_list.Add(prop_array[prop_array_index]);
                            //}
                        }
                        else
                        {
                            prop_list.Add(reader.GetName(i) + ":" + reader.GetValue(i));
                        }
                    }

                    //string prop_str = 

                    SkillStaticData skill = new SkillStaticData();
                    skill.prop = new Prop(prop_list);
                    skill.id = skill.prop.GetUint(PropertiesKey.SKILL_ID);
                    skill.name = skill.prop.GetString(PropertiesKey.SKILL_NAME);
                    skill.cd = skill.prop.GetFloat(PropertiesKey.SKILL_CD);
                    skill.skill_time = skill.prop.GetFloat(PropertiesKey.SKILL_TIME);
                    skill.prop.Remove(PropertiesKey.SKILL_ID);
                    skill.prop.Remove(PropertiesKey.SKILL_NAME);
                    skill.prop.Remove(PropertiesKey.SKILL_CD);
                    skill.prop.Remove(PropertiesKey.SKILL_PROP);
                    skill.prop.Remove(PropertiesKey.SKILL_TIME);
                    m_Datas.Add(skill.id, skill);
                }
            }

        }

        public Dictionary<uint, SkillStaticData> data
        {
            get
            {
                return m_Datas;
            }
        }

        public SkillStaticData Get(uint id)
        {
            if(m_Datas.ContainsKey(id))
            {
                return m_Datas[id];
            }
            else
            {
                Debug.LogError("Don't have the id !!!");
                return null;
            }
        }

#if UNITY_EDITOR
        private string[] m_skill_list = null;
        public int GetSkillCount
        {
            get
            {
                return m_Datas.Count;
            }
        }

        public string[] GetSkillIsList
        {
            get
            {
                if (m_skill_list == null)
                {
                    ReaderSkillList();
                }
                return m_skill_list;
            }
        }

        public void ReaderSkillList()
        {
            m_skill_list = new string[m_Datas.Count];

            int index = 0;
            foreach (var it in m_Datas)
            {
                m_skill_list[index] = it.Key.ToString();
                index++;
            }
        }

        public void AddNewSkill(SkillStaticData data)
        {
            if(m_Datas.ContainsKey(data.id))
            {
                EditorUtility.DisplayDialog("错误","ID错误，请检查！！！","确定");
            }
            else
            {
                m_Datas.Add(data.id,data);
            }
        }

#endif

    }
}
