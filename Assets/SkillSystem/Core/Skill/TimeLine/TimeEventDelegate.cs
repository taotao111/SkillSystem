using UnityEngine;
using System.Collections;
using System;

public class TimeEventDelegate : TimeEvent {
    public Action eventDelegate;

    public TimeEventDelegate()
    : base ()
    { }

    public void Add(Action dele)
    {
        eventDelegate += dele;
    }

    public void Remove(Action dele)
    {
        eventDelegate -= dele;
    }

    public override void Trigger()
    {
        base.Trigger();
        if(eventDelegate != null)
        {
            eventDelegate();
        }
    }
}
