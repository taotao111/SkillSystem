using UnityEngine;
using System.Collections.Generic;
public class EffectManager
{
    private List<Effect> m_Effects = new List<Effect>();
    public void Init() { }
    public Effect Create(string name, Vector3 postion,Quaternion rotation) 
    {
        return Create(name, postion, rotation, float.MaxValue);
    }
    public Effect Create(string name, Vector3 postion, Quaternion rotation, float duration)
    {
        //
        Debug.LogError("加载特效资源");
        //Effect effect = new Effect(GameCenter.Instance.ResourcesManager.Instantiate<GameObject>(URL.EffectSkillPath + name, postion, rotation),duration);
        Effect effect = new Effect(GameObject.Instantiate(Resources.Load(URL.EffectSkillPath + name), postion, rotation) as GameObject, duration);
        m_Effects.Add(effect);

        return effect;
    }
    public void Update(float elapsed_sec)
    {
        for (int i = m_Effects.Count; i >= 0; i--)
        {
            if(m_Effects[i].IsExpire)
            {
                m_Effects[i].DestroyImmediate();
                m_Effects.RemoveAt(i);
            }
        }
    }
}
