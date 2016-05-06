using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioManager
{
    private AudioListener m_AudioListener;
    private GameObject m_HanderObject;
    private Dictionary<eAudioLayer, AudioLayer> m_AudioLayer = new Dictionary<eAudioLayer, AudioLayer>();
    public void Init()
    {
        m_HanderObject = new GameObject("AudioListener");

        //----------------------清除现有场景里面的AudioListener组件---------------------------------------------------
        AudioListener[] listens = GameObject.FindObjectsOfType<AudioListener>();

        for (int i = 0; i < listens.Length; i++)
        {
            Object.Destroy(listens[i]);
        }

        listens = null;



        //-----------------------------------------------------------
        m_AudioListener = m_HanderObject.AddComponent<AudioListener>();



        //------------------------------------------------------------------------
        GameObject.DontDestroyOnLoad(m_HanderObject);


        AddAudioLayer(eAudioLayer.BACKGROUND, 1);
    }

    /// <summary>
    /// 添加音效
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="count"></param>
    public void AddAudioLayer(eAudioLayer audio_layer, int audio_count = 1)
    {
        //------------------判断当前是否已有层次，-----------------------------
        if (m_AudioLayer.ContainsKey(audio_layer))
        {
            m_AudioLayer[audio_layer].Clear();

            m_AudioLayer.Remove(audio_layer);
        }

        //---------------------添加新层次---------------------------------
        AudioLayer newLayer = new AudioLayer(this);
        newLayer.Create(audio_count);

        m_AudioLayer.Add(audio_layer, newLayer);
    }

    public AudioSource AddAudioSource()
    {
        return m_HanderObject.AddComponent<AudioSource>();
    }

    public void SetLoop(eAudioLayer audio_layer,bool is_loop)
    {
        if (m_AudioLayer.ContainsKey(audio_layer))
        {
            m_AudioLayer[audio_layer].SetLoop(is_loop);
        }
    }
    public void SetVolume(eAudioLayer audio_layer, float audio_volume)
    {
        if(m_AudioLayer.ContainsKey(audio_layer))
        {
            m_AudioLayer[audio_layer].SetVolume(audio_volume);
        }
    }

    public void Play(eAudioLayer audio_layer, string audio_name)
    {
        if(m_AudioLayer.ContainsKey(audio_layer))
        {
            m_AudioLayer[audio_layer].Play(audio_name);
        }
    }
}

public class AudioLayer 
{
    private AudioManager m_AudioManager;
    private Queue<AudioSource> m_AudioSources = new Queue<AudioSource>();

    public AudioLayer(AudioManager audio_manager)
    {
        m_AudioManager = audio_manager;
    }

    public void Create(int audio_count, bool is_loop = false, float audio_volume = 1)
    {
        while(audio_count > 0)
        {
            AudioSource audio_sources = m_AudioManager.AddAudioSource();
            audio_sources.loop = is_loop;
            audio_sources.volume = audio_volume;
            m_AudioSources.Enqueue(audio_sources);
            audio_count--;
        }
    }

    public void SetVolume(float audio_volume)
    {
        foreach (AudioSource it in m_AudioSources)
        {
            it.volume = audio_volume;
        }
    }

    public void SetLoop(bool is_loop)
    {
        foreach (AudioSource it in m_AudioSources)
        {
            it.loop = is_loop;
        }
    }

    public void Clear() 
    {
        while(m_AudioSources.Count > 0)
        {
            Object.Destroy(m_AudioSources.Dequeue());
        }
    }

    public void Play(string audio_name) 
    {
        if (string.IsNullOrEmpty(audio_name)) { return; }
    }
}