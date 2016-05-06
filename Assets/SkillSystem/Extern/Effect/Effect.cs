using UnityEngine;
using System.Collections;

public class Effect {
    public Transform transform;

    public bool IsExpire { get; private set; }

    private float m_Duration = float.MaxValue;

    public Effect(GameObject obj, float duration)
    {
        if (obj == null)
        {
            IsExpire = true;
        }
        else
        {
            transform = obj.GetComponent<Transform>();
            if(duration <= 0)
            {
                m_Duration = GetDuration();
            }
            else
            {
                m_Duration = duration;
            }
        }

    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="elapsed_sec"></param>
    public void Update(float elapsed_sec)
    {
        if(!IsExpire)
        {
            m_Duration -= elapsed_sec;
            if (m_Duration <= 0)
            {
                DestroyImmediate();
                IsExpire = true;
            }
        }
    }
    /// <summary>
    /// 获得特效时间
    /// </summary>
    /// <returns></returns>
    private float GetDuration()
    {
        float duration = 0;

        if (transform != null)
        {
            ParticleSystem[] particles = transform.GetComponentsInChildren<ParticleSystem>();

            for (int i = 0; i < particles.Length; i++ )
            {
                if(particles[i].duration > duration)
                {
                    duration = particles[i].duration;
                }
            }
        }
        else
        {
            IsExpire = true;
        }

        return duration;
    }
    /// <summary>
    /// 停止特效播放，软销毁
    /// </summary>
    public void Stop()
    {
        if (transform != null)
        {
            ParticleSystem[] particles = transform.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].loop = false;
                //particles[i].enableEmission = false;
                particles[i].Stop();
            }
        }
        else
        {
            IsExpire = true;
        }
    }
    /// <summary>
    /// 软销毁，取消特效的循环播放，获得特效中持续时间最长的特效时间进行销毁
    /// </summary>
    public void Destroy()
    {
        m_Duration = GetDuration();
        Stop();
    }
    /// <summary>
    /// 立即销毁特效
    /// </summary>
    public void DestroyImmediate()
    {
        IsExpire = true;
        if (transform)
        {
            GameObject.DestroyImmediate(transform.gameObject);
            transform = null;
        }
    }
    public void SetPosition(Vector3 position, bool islocal = false)
    {
        if (transform == null)
        {
            IsExpire = true;
        }
        else
        {
            if (islocal) { transform.localPosition = position; }
            else { transform.position = position; }
        }
    }
    public void SetRotation(Quaternion rotation, bool islocal = false)
    {
        if (transform == null)
        {
            IsExpire = true;
        }
        else
        {
            if (islocal) { transform.localRotation = rotation; }
            else { transform.rotation = rotation; }
        }
    }
    public void SetEulerAngles(Vector3 eulerangles, bool islocal = false)
    {
        if (transform == null)
        {
            IsExpire = true;
        }
        else
        {
            if (islocal) { transform.localEulerAngles = eulerangles; }
            else { transform.eulerAngles = eulerangles; }
        }
    }
    public void SetParent(Transform parent, bool reset = true)
    {
        transform.parent = parent;
        if (reset)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
    public void AddComponent<T>() where T : Component
    {
        if (transform != null)
        {
            transform.gameObject.AddComponent<T>();
        }
    }
}
