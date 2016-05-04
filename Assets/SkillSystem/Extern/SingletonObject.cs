using System;
using System.Collections.Generic;
/// <summary>
/// 单例模式，创建单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonObject<T>
{
    private static T m_Singleton;

    public static T Instance
    {
        get
        {
            if (m_Singleton == null)
            {
                m_Singleton = (T)Activator.CreateInstance(typeof(T));
            }
            return m_Singleton;
        }
    }

}