using UnityEngine;
using System.Collections;

public class GameCenter : SystemBase<GameCenter>  {
    private EffectManager m_EffectManager;
    private DataManager m_DataManager;
    private SceneManager m_SceneManager;
    private AudioManager m_AudioManager;
    /// <summary>
    /// 特效管理类
    /// </summary>
    public EffectManager EffectManager
    {
        get
        {
            if(m_EffectManager == null)
            {
                m_EffectManager = new EffectManager();
                m_EffectManager.Init();
            }
            return m_EffectManager;
        }
    }

    /// <summary>
    /// 数据管理，底层数据
    /// </summary>
    public DataManager DataManager
    {
        get
        {
            if (m_DataManager == null)
            {
                m_DataManager = new DataManager();
                m_DataManager.Init();
            }

            return m_DataManager;
        }
    }

    public SceneManager SceneManager
    {
        get
        {
            if (m_SceneManager == null)
            {
                m_SceneManager = new SceneManager();
                m_SceneManager.Init();
            }

            return m_SceneManager;
        }
    }

    public AudioManager AudioManager
    {
        get
        {
            if (m_AudioManager == null)
            {
                m_AudioManager = new AudioManager();
                m_AudioManager.Init();
            }

            return m_AudioManager;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Init()
    {
        m_EffectManager = new EffectManager();
        m_EffectManager.Init();

        m_DataManager = new DataManager();
        m_DataManager.Init();

        m_SceneManager = new SceneManager();
        m_SceneManager.Init();

        m_AudioManager = new AudioManager();
        m_AudioManager.Init();
    }
}
