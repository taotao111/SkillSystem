using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Code.SkillSystem.Runtime;
public class Character : MonoBehaviour, ISkillCaster, IMessageListener
{
    #region Pubilc
    /// <summary>
    /// 阵营
    /// </summary>
    public eCharacterLayer Layer{ get { return m_Layer; } set { m_Layer = value; } }
    public uint id;
    #endregion

    #region Protected

    protected Transform m_Transform;
    protected Dictionary<string, Transform> m_ChildrenNodes = new Dictionary<string, Transform>();
    protected SkillHub skillHub;
    [SerializeField]
    private eCharacterLayer m_Layer = eCharacterLayer.None;
    #endregion

    
    public void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_ChildrenNodes = gameObject.GetChildrenNodes();
        skillHub = new SkillHub(this);
        Init();
    }

    #region Override
    public virtual void Init() { }

    #endregion

    #region ISkillCaster
    public Hashtable GetParams()
    {
        throw new System.NotImplementedException();
    }

    public void HandleSkill(Hashtable param)
    {
        throw new System.NotImplementedException();
    }
    public void HitHandle(Hashtable param)
    {
        throw new System.NotImplementedException();
    }
    public Transform Transform
    {
        get { return m_Transform; }
    }
    public Transform GetMount(string name)
    {
        if(m_ChildrenNodes.ContainsKey(name))
        {
            return m_ChildrenNodes[name];
        }
        else
        {
            return m_Transform;
        }
    }
    #endregion

    #region IBuffHolder
    #endregion

    #region IMessageListener
    public void HandleMessage(IMessage message)
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
