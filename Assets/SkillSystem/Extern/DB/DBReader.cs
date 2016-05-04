using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using Mono.Data.Sqlite;

public class DBReader {
    protected string m_FileName;
    protected List<Prop> db = new List<Prop>();
    public DBReader(string file_name)
    {
        m_FileName = file_name;
        InitSqlite();
    }
    public DBReader(TextAsset text_asset)
    {
        m_FileName = text_asset.name;
        InitXml(text_asset);
    }
    public virtual void InitSqlite()
    {
        using (SqliteDataReader reader = LocalDB.instance.ExecuteReader("select * from " + m_FileName))
        {
            while (true)
            {
                if (!reader.Read())
                    break;

                Prop prop = new Prop();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals(PropertiesKey.MOTION_PROP))
                    {
                        prop.Add(reader.GetValue(i).ToString().Split(';'));
                    }
                    else
                    {
                        prop.Add(reader.GetName(i), reader.GetValue(i).ToString());
                    }
                }

                db.Add(prop);
            }
        }
    }

    public virtual void InitXml(TextAsset textAsset)
    {

    }
}
