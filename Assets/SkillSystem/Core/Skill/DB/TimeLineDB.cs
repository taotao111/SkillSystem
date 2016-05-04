using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Reflection;
using System;

public class TimeLineDB : DBReader
{
    //private List<TimeEvent> m_Datas = new List<TimeEvent>();
    private Dictionary<uint, List<SqliteDataReader>> m_Datas = new Dictionary<uint, List<SqliteDataReader>>();

    public TimeLineDB(string file_name)
        : base(file_name)
    {

    }
    public override void InitSqlite()
    {
        using (SqliteDataReader reader = LocalDB.instance.ExecuteReader("select * from " + m_FileName))
        {
            while (true)
            {
                if (!reader.Read())
                    break;
                //LocalDB.instance.connection.
                //Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 

                //TimeEvent time_event = assembly.CreateInstance(((TimeEventType)((int)reader[PropertiesKey.TIMELINE_EVENT_TYPE])).ToString()) as TimeEvent;
                //time_event.Convert(reader);

                //m_Datas.Add(time_event);

                //int temp_id = (int)reader[PropertiesKey.TIMELINE_OWNER];


                //uint id = (uint)(int)reader[PropertiesKey.TIMELINE_OWNER];

                //if (!m_Datas.ContainsKey(id))
                //{
                //    m_Datas.Add(id, new List<SqliteDataReader>());
                //}

                //m_Datas[id].Add(reader);
            }
        }
    }
    public List<TimeEvent> GetTimeLineEvents(uint id)
    {
        List<TimeEvent> get_value = new List<TimeEvent>();

        using (SqliteDataReader reader = LocalDB.instance.ExecuteReader("select * from " + m_FileName + " where owner = " + id))
        {
            while (true)
            {
                if (!reader.Read())
                    break;
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 

                TimeEvent time_event = assembly.CreateInstance(((TimeEventType)((int)reader[PropertiesKey.TIMELINE_EVENT_TYPE])).ToString()) as TimeEvent;
                time_event.Convert(reader);

                get_value.Add(time_event);
            }
        }

        return get_value;
    }

}
