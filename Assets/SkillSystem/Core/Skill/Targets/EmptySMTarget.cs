using UnityEngine;
using System.Collections;
using System;

public class EmptySMTarget : MonoBehaviour,ISkillTarget {
    public eCharacterLayer Layer
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public Transform Transform
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public Transform GetMount(string name)
    {
        return transform;
    }

    public void HandleSkill(Hashtable param)
    {
        throw new NotImplementedException();
    }

    public void HitHandle(Hashtable param)
    {
        throw new NotImplementedException();
    }
}
