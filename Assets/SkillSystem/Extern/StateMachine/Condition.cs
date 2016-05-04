using UnityEngine;
using System.Collections;

public class Condition 
{
    public virtual void Init() { }
    public virtual void Update(float elapsed_sec) { }

    public virtual bool Check()
    {
        return true;
    }
}
