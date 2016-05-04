using UnityEngine;
using System.Collections;

public class FollowObjComp : MonoBehaviour {

    private Transform m_Follow;
    private Vector3 m_Offset = Vector3.zero;
    private Transform m_Tran;
    public void SetFollow(Transform follow,Vector3 offset)
    {
        m_Follow = follow;
        m_Offset = offset;
        m_Tran = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        if(m_Follow != null)
        {
            m_Tran.position = m_Follow.position + m_Offset;
        }
    }
}
