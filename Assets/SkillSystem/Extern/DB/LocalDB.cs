using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.External.Engine.Sqlite;
using System;
public class LocalDB : SqliteDatabase
{
    //数据库表结构版本号，递增
    private const int m_Version = 1;
    //数据库文件路径
    private static string m_FilePath = URL.DBPath;
    private static LocalDB m_Instance;
    private bool isDone;
    private Exception error;
    public static LocalDB instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new LocalDB();
            }

            if (!m_Instance.IsOpened)
            {
                m_Instance.OpenFromMemory();
            }

            return m_Instance;
        }
        set
        {
            if (m_Instance != null)
            {
                m_Instance.Close();
                m_Instance = null;
            }
        }
    }

    public LocalDB ()
        : base(m_FilePath, m_Version)
    { }

    public T[] ReadTable<T>(string table_name) where T : DBDataBase, new()
    {
        T[] read = (T[])instance.ExecuteQuery<T>("select * from " + table_name).ToArray();
        return read;
    }

    public void UpdateTable(string tabel_name)
    {

    }

    public void CreateTable<T>(string table_name, List<T> data) where T : DBDataBase
    {
        ExecuteNonQuery("DROP TABLE IF EXISTS " + table_name);
#if UNITY_EDITOR
        //创建表
        string table_format = "CREATE TABLE [{0}] ({1})";

        System.Text.StringBuilder table_property = new System.Text.StringBuilder();

        Dictionary<string, string> m_property = new Dictionary<string,string>();
        

        if (data.Count <= 0) { return; }

        for (int j = 0; j < data.Count; j++ )
        {
            System.Reflection.FieldInfo[] properties = data[j].GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.GetField);

            for (int i = 0; i < properties.Length; i++)
            {
                //DBMemberAttribute db_attribute = (DBMemberAttribute)properties[i].FieldType.GetCustomAttributes(typeof(DBMemberAttribute), true).FirstOrDefault();
                DBMemberAttribute db_attribute = (DBMemberAttribute)(DBMemberAttribute.GetCustomAttribute(properties[i], typeof(DBMemberAttribute)));

                if (db_attribute != null && !m_property.ContainsKey(db_attribute.Name))
                {
                    m_property.Add(db_attribute.Name, db_attribute.dbType);
                }
            }
        }


        int index = 0;
        foreach (var it in m_property)
        {
            table_property.Append(string.Format("[{0}] {1}", it.Key, it.Value));

            if(index < m_property.Count - 1){table_property.Append(",");}
            index++;
        }

        ExecuteNonQuery(string.Format(table_format, table_name, table_property.ToString()));
        ////写入数据
        WriteToTable(table_name, data);
#endif
    }

    internal void ExecuteReader()
    {
        throw new NotImplementedException();
    }

    public void WriteToTable<T>(string table_name, List<T> data) where T : DBDataBase
    {
        string data_format = "insert into {0} ({1}) values({2})";

        List<object> properties_list = new List<object>();

        for (int j = 0; j < data.Count; j++)
        {
            properties_list.Clear();

            System.Text.StringBuilder names = new System.Text.StringBuilder();
            System.Text.StringBuilder values = new System.Text.StringBuilder();
            System.Reflection.FieldInfo[] properties = data[j].GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.GetField);
            for (int i = 0; i < properties.Length; i++)
            {
                //DBMemberAttribute db_attribute = (DBMemberAttribute)properties[i].FieldType.GetCustomAttributes(typeof(DBMemberAttribute), true).FirstOrDefault();
                DBMemberAttribute db_attribute = (DBMemberAttribute)(DBMemberAttribute.GetCustomAttribute(properties[i], typeof(DBMemberAttribute)));

                if (db_attribute != null)
                {
                    names.Append(db_attribute.Name);
                    names.Append(",");

                    values.Append("@" + db_attribute.Name);
                    values.Append(",");

                    //object prop = properties[i].GetValue(data[j]);
                    //if (prop != null)
                    //{
                    //    properties_list.Add(properties[i].GetValue(data[j]).ToString());
                    //}
                    //else
                    //{
                    //    properties_list.Add(properties[i].GetValue(data[j]));
                    //}
                    properties_list.Add(properties[i].GetValue(data[j]));
                }
            }

            string end_names = names.ToString();
            string end_values = values.ToString();
            if(end_names.EndsWith(","))
            {
                end_names = end_names.Substring(0, end_names.Length - 1);
            }
            if (end_values.EndsWith(","))
            {
                end_values = end_values.Substring(0, end_values.Length - 1);
            }

            ExecuteNonQuery(string.Format(data_format, table_name, end_names, end_values), properties_list.ToArray());
        }
        
    }

    public void ClearTableData(string table_name)
    {
        ExecuteNonQuery("TRUNCATE TABLE " + table_name);
    }

    protected override void CreateDatabase()
    {
        
    }

    protected bool CheckErorr(SqliteDatabase db)
    {
        if (db.HasError)
        {
            error = db.LastError;
            isDone = true;
            return true;
        }
        return false;
    }
}
