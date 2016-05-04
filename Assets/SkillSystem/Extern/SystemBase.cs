using UnityEngine;
using System.Collections;

public class SystemBase<T> : SingletonObject<T>
{
    public virtual void Init() { }
}
